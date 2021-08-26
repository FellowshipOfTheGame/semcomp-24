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
    const finishedAt = new Date().toISOString()

    let newPersonalRecord = false

    // Request Fields validations
    if(Number.isNaN(score)) return res.status(400).json({ message: "invalid field @score" })
    if(Number.isNaN(gold)) return res.status(400).json({ message: "invalid field @gold" })
    if(!nonce) return res.status(400).json({ message: "invalid field @nonce" })
    if(!sign) return res.status(400).json({ message: "invalid field @signature" })

    // Verifying the signature 
    const reqSign = createHmac('sha256', config.REQUEST_SIGNATURE_KEY).update(JSON.stringify({score, gold, nonce, sign: ""})).digest('base64')
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
            doc.goldAcc += gold
            doc.runs += 1
            newPersonalRecord = doc.topScore < score
            doc.topScore = (newPersonalRecord) ? score : doc.topScore
            doc.topScoreDate = (newPersonalRecord) ? new Date().toISOString() : doc.topScoreDate
            return doc.save() 
        })
        .then(( ) => new RaceModel({ userId, score, gold, startedAt, finishedAt}).save())
        .then(( ) => { 
            return res.json({ message: "ok", isPersonalRecord: newPersonalRecord }) 
        })
        .catch((err) => { 
            console.error(`at /race/finish: Error saving user ${userId} race! ${err}`)
            return res.status(500).json({ message: "internal server error" })
        })
    })
}

async function ranking(req, res) {

    // Using lean + indexed search to optimize performance
    UserModel.find().lean()
    .select('name topScore topScoreDate')
    .sort({topScore: -1, topScoreDate: 1})
    .exec()
    .then((ranking) => {

        let personal = { position: -1, topScore: -1 }
        
        if(req.user?._id !== undefined) { 
            const userPos = ranking.findIndex((u) => req.user._id.equals(u._id))

            if(userPos >= 0){ 
                personal.position = userPos+1
                personal.topScore = ranking[userPos].topScore
            }
        }

        return res.json({ 
            message: "ok", 
            personal: personal,
            rank: ranking.map((u) => { 
                return { 
                    topScore: u.topScore,
                    name: u.name,
                    topScoreDate: u.topScoreDate
                }
            })
        })
    })
    .catch((err) => { 
        console.error(`at /race/ranking: error loading ranking ${err}`) 
        return res.status(500).json({ message: "internal server error" })
    })
}