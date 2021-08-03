// Dependencies
const Redis = require("ioredis")
const { randomInt, createHmac } = require("crypto")

const config = require("../config/")
const UserModel = require("../models/User")
const RaceModel = require("../models/Race")
const RankingModel = require("../models/Ranking")

// Starting Races Caching DB
const redis = new Redis({ 
    port: config.REDIS_PORT, 
    host: config.REDIS_HOST,
}); 

// Exporting controller async functions
module.exports = { 
    start,
    finish,
    ranking
}

// Init global ranking variable 
let globalRank = {rank: [], minScore: -1, updatedAt: new Date(), createdAt: new Date()}
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
            doc.topScore      = (newPersonalRecord) ? score : doc.topScore
            return doc.save() 
        })
        .then(( ) => new RaceModel({ userId, score, gold, startedAt, finishedAt}).save())
        .then(async ( ) => { 
            if( globalRank.minScore < score ) 
                await updateRanking(userId, req.user.name, score)

            return res.json({ message: "ok", isPersonalRecord: newPersonalRecord }) 
        })
        .catch((err) => { 
            console.error(`at /race/finish: Error saving user ${userId} race! ${err}`)
            return res.status(500).json({ message: "internal server error" })
        })
    })
}

async function ranking(req, res) {

    // await new UserModel({  
    //     google_id: "abcde", 
    //     name: "GabrielVanLoon", 
    //     nickname: "GabrielVanLoon",
    //     email: "gabriel@teste.com",
    //     created_at: new Date()
    // }).save()

    return res.json({ message: "ok", ranking: globalRank })
}

// Auxiliar Functions 
async function loadRanking(){
    await RankingModel.findOne().sort({createdAt: -1}).limit(1).exec()
    .then((doc) => {
        if(!doc) return Promise.resolve({ rank: [], minScore: 0 }) 
        else     return Promise.resolve({ rank: doc.rank, minScore: doc.minScore })
    }) 
    .then((data) => { globalRank = new RankingModel(data) })
    .catch((err) => { console.log(`at loadRanking(): error loading ranking ${err}`) })
}

async function updateRanking(userId, nickname, score){ 
    // If user is already in ranking then update his score
    let isInRanking = false
    for(let i = 0; isInRanking === false && i < globalRank.rank.length; i++){
        if(globalRank.rank[i].userId.equals(userId)){
            if(score < globalRank.rank[i].score)
                return 
            isInRanking = true
            globalRank.rank[i].score = score
        }
    }

    // If not in ranking need to append to the ranking list
    if(isInRanking === false) 
        globalRank.rank.push({userId, nickname, score})
    
    // Reordering the ranking
    globalRank.rank.sort((a,b) => (b.score > a.score) ? 1 : -1)

    // Remove ranking overflow
    globalRank.rank = globalRank.rank.slice(0,10)
    
    // Update min score and update time
    globalRank.minScore = globalRank.rank.reduce((cur,item) => (item.score < cur) ? item.score : cur, Number.MAX_SAFE_INTEGER)
    globalRank.updatedAt = new Date()

    return await globalRank.save()
}
