// Dependencies
const Redis = require("ioredis")
const { randomInt, createHmac } = require("crypto")

const config = require("../config/")
const UserModel = require("../models/User")
const RaceModel = require("../models/Race")
const { nonceClient: redis } = require("../loaders/redis")

// Exporting controller async functions
module.exports = { 
    start,
    finish,
    ranking
}

// Init global ranking variable
const rankingMazSize = 10
let   globalRank     = { rank: [], minScore: -1, updatedAt: new Date() }
loadRanking()

// Controller Functions
async function start(req, res) {
    const userId = req.user._id
    const nonce  = randomInt(10000000, 99999999).toString();
    const startedAt = new Date().toISOString()

    redis.multi()
    .set(`${userId}-nonce`, nonce)
    .set(`${userId}-startedAt`, startedAt)
    .exec((err, results) => {

        if(err){
            console.error(`at /race/start: Error in set nonce ${nonce} to user ${userId}`)
            return res.status(500).json({ message: "internal server error" })
        }

        return res.json({ message: "ok", nonce: nonce })
    })    
}

async function finish(req, res) { 
    const userId = req.user._id
    const score = parseInt(req.body?.score)
    const gold  = parseInt(req.body?.gold)
    const nonce = req.body?.nonce?.toString().trim()
    const sign  = req.body?.sign?.toString().trim()
    const finishedAt = new Date().toISOString().trim()

    let newPersonalRecord = false

    // Request Fields validations
    if(Number.isNaN(score)) return res.status(400).json({ message: "invalid field @score" })
    if(Number.isNaN(gold)) return res.status(400).json({ message: "invalid field @gold" })
    if(!nonce) return res.status(400).json({ message: "invalid field @nonce" })
    if(!sign) return res.status(400).json({ message: "invalid field @signature" })

    // Verifying the signature 
    const reqSign = createHmac('sha256', config.REQUEST_SIGNATURE_KEY).update(JSON.stringify({score, gold, nonce})).digest('hex')
    if(sign !== reqSign) return res.status(400).json({ message: "incorrect signature" })

    // Get nonce and start time in Cache Database
    redis.multi()
    .get(`${userId}-nonce`)
    .get(`${userId}-startedAt`)
    .del(`${userId}-nonce`)
    .exec((err, results) => {
    
        if(err){
            console.error(`at /race/finish: Error in get nonce in cache to user ${userId}`)
            return res.status(500).json({ message: "internal server error" })
        }
        
        const storedNonce = results[0][1]
        const startedAt = results[1][1]

        if(storedNonce === null || storedNonce !== nonce){
            console.log(`at /race/finish: Invalid Nonce ${nonce} != ${storedNonce} to user ${userId}`)
            return res.status(400).json({ message: "incorrect nonce" })
        }

        UserModel.findById(userId)
        .then((doc) => { 
            doc.gold += gold
            doc.runs += 1
            newPersonalRecord = doc.topScore < score
            doc.topScore = (newPersonalRecord) ? score : doc.topScore
            return doc.save() 
        })
        .then(( ) => new RaceModel({ userId, score, gold, startedAt, finishedAt}).save())
        .then(( ) => { 
            if( newPersonalRecord && globalRank.minScore < score ) 
                updateRanking(userId, req.user.name, score)

            return res.json({ message: "ok", isPersonalRecord: newPersonalRecord }) 
        })
        .catch((err) => { 
            console.error(`at /race/finish: Error saving user ${userId} race! ${err}`)
            return res.status(500).json({ message: "internal server error" })
        })
    })
}

async function ranking(req, res) {
    return res.json({ 
        message: "ok",
        updatedAt: globalRank.updatedAt,
        rank: globalRank.rank.map((item) => { 
            return { 
                name: item.name, 
                nickname: item.nickname, 
                topScore: item.topScore
            }
        }) 
    })
}

// Auxiliar Functions 
async function loadRanking(){
    await UserModel.find().select('name nickname topScore').sort({topScore: -1}).limit(rankingMazSize).exec()
    .then((docs) => {
        const N             = docs.length
        globalRank.rank     = docs
        globalRank.minScore = (N >= rankingMazSize) ? globalRank.rank[N-1].topScore : -1
    })
    .catch((err) => { console.error(`at loadRanking(): error loading ranking ${err}`) })
}

function updateRanking(_id, nickname, topScore){ 
    // Flag to know if user is already in ranking
    let isInRanking = false

    // If user is already in ranking then update his score
    for(let i = 0; isInRanking === false && i < globalRank.rank.length; i++){
        if(_id.equals(globalRank.rank[i]._id)){
            if(topScore < globalRank.rank[i].topScore) 
                return  // If he did not beat his own best topScore then ignore
            
            isInRanking = true
            globalRank.rank[i].topScore = topScore
        }
    }

    // If not in ranking need to append to the ranking list
    if(isInRanking === false) 
        globalRank.rank.push({_id, nickname, topScore})
    
    // Reordering the ranking
    globalRank.rank.sort((a,b) => (b.topScore > a.topScore) ? 1 : -1)

    // Remove ranking overflow
    globalRank.rank = globalRank.rank.slice(0,rankingMazSize)
    
    // Update min score and update time
    const N = globalRank.rank.length
    globalRank.minScore  = (N >= rankingMazSize) ? globalRank.rank[N-1].topScore : -1
    globalRank.updatedAt = new Date()
}
