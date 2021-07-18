// Dependencies
const Redis = require("ioredis")
const { randomInt } = require("crypto")

const config = require("../config/")
const UserModel = require("../models/User")
const RaceModel = require("../models/Race")

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

// Controller Functions
async function start(req, res) {
    const userId = "60f32e0105e0c858b8746d75"
    const nonce  = randomInt(10000000, 99999999).toString();
    const startedAt = new Date().toISOString()

    redis.multi()
    .set(`${userId}-nonce`, nonce)
    .set(`${userId}-startedAt`, startedAt)
    .exec((err, results) => {

        if(err){
            console.error(`at /race/start: Error in set nonce ${nonce} to user ${userId}`)
            return res.status(500).json({ message: "Internal Server Error!" })
        }

        return res.json({ message: "ok", nonce: nonce })
    })    
}

async function finish(req, res) { 

    const userId = "60f32e0105e0c858b8746d75"
    const score = parseInt(req.body?.score)
    const gold  = parseInt(req.body?.gold)
    const nonce = req.body?.nonce?.toString().trim()
    const sign  = req.body?.sign?.toString().trim()
    const finishedAt = new Date().toISOString().trim()

    // Request Fields validations
    if(Number.isNaN(score)) return res.status(400).json({ message: "Invalid field score!" })
    if(Number.isNaN(gold)) return res.status(400).json({ message: "Invalid field gold!" })
    if(!nonce) return res.status(400).json({ message: "Invalid field nonce!" })
    if(!sign) return res.status(400).json({ message: "Invalid field sign!" })

    // Get nonce and start time in Cache Database
    redis.multi()
    .get(`${userId}-nonce`)
    .get(`${userId}-startedAt`)
    .del(`${userId}-nonce`)
    .exec((err, results) => {
    
        if(err){
            console.error(`at /race/finish: Error in get nonce in cache to user ${userId}`)
            return res.status(500).json({ message: "Internal Server Error!" })
        }
        
        const storedNonce = results[0][1]
        const startedAt = results[1][1]

        if(storedNonce === null || storedNonce !== nonce){
            console.log(`at /race/finish: Invalid Nonce ${nonce} != ${storedNonce} to user ${userId}`)
            return res.status(400).json({ message: "Invalid nonce!" })
        }
        
        // TODO: estudar possibilidade de utilizar transactions p/ diminuir latencia
        //       ou realizar cache dos dados do usuario?
        
        // TODO: Adicionar usuários no ranking se entrarem no top 10.

        UserModel.findById(userId)
        .then((doc) => { doc.gold += gold; return doc.save(); })
        .then(( ) => { return new RaceModel({ userId, score, gold, startedAt, finishedAt}).save() })
        .then(( ) => { return res.json({ message: "ok" }) })
        .catch((err) => { 
            console.error(`at /race/finish: Error saving user ${userId} race!`)
            return res.status(500).json({ message: "Internal Server Error!" })
        })
    })
}

async function ranking(req, res) {

    // const newUser = new UserModel({
    //     created_at: new Date(),
    //     google_id: "google-id-fake-lol",
    //     name: "Gabriel",
    //     email: "email@teste.com",
    // })
    // newUser.save()

    // console.log("Saving foo-bar")
    // redis.set("foo", "bar")

    // redis.get("foo")
    // .then(function (result) {
    //     console.log(`Returned ${result}`); // Prints "bar"
    // });
    
    // redis.multi()
    // .set(`teste33`, 'teste33-foo')
    // .get(`teste33`)
    // .get(`teste44`)
    // .del(`teste33`)
    // .get(`teste33`)
    // .exec((err, results) => { console.log(results[0])})

    return res.json({ serverStatus: "OK", endpoint: "race ranking" })
}