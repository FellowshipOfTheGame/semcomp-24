// Dependencies
const Redis = require("ioredis");
const { randomInt } = require("crypto");
const config = require("../config/")

const redis = new Redis({ port: config.REDIS_PORT, host: config.REDIS_HOST}); 

// Exporting controller async functions
module.exports = { 
    start,
    finish,
    ranking
}

// Controller Functions
async function start(req, res) {
    // Generate Nonce, attach to user and return it
    const userId = "user-auth-123abc"
    const nonce = randomInt(10000000, 99999999).toString();
    
    redis.set(`${userId}-nonce-${nonce}`, nonce)
    .then((r) => { console.log(`at /race/start: Nonce ${nonce} generate to user ${userId}`) })
    .catch((e) => { console.log(`at /race/start: Error in set nonce ${nonce} to user ${userId}`) })

    return res.json({ message: "ok", nonce: nonce })
}

async function finish(req, res) { 

    console.log(req.body)

    // Get userId to retrieve the nonce and verify it
    const userId = "user-auth-123abc"
    const nonce = req.body?.nonce.toString()
    const score = parseInt(req.body?.score)
    const hash  = req.body?.hash

    // Basic fields validations
    if( !nonce || !score || !hash || !Number.isInteger(score)) 
        return res.status(400).json({ message: "Incorrect parameters." })

    // TODO: check JSON string with generated hash :P
    
    redis.multi()
    .get(`${userId}-nonce-${nonce}`)
    .del(`${userId}-nonce-${nonce}`)
    .exec((err, results) => {
        const storedNonce = results[0][1]

        // Invalid Nonce
        if(storedNonce === null || storedNonce !== nonce){
            console.log(`at /race/finish: Invalid Nonce ${nonce} != ${storedNonce} to user ${userId}`)
            return res.status(400).json({ message: "Invalid nonce!" })
        }
        
        // TODO: Store races into mongo and verify if is in top 10 ranking 
        
        return res.json({ message: "ok" })
    })
}

async function ranking(req, res) {

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