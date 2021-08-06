// Dependencies
const Redis = require("ioredis")
const passport = require('passport')
const { v4: uuidv4, validate: uuidValidate } = require('uuid')

const config = require("../config/")

// Exporting controller async functions
module.exports = { 
    loginCallback,
    getSession,
    logout,
}

// Starting One Time Password Caching DB
const redis = new Redis({ 
    port: config.REDIS_PORT, 
    host: config.REDIS_HOST,
}) 

// Controller Functions
async function loginCallback(req, res) { 
    const otpCode = uuidv4()

    redis.set(`otp-${otpCode}`, req.sessionID, "EX", 3*60, (err) => { // Expire in 3 minutes        
        if(err){
            console.error(`at /session/login/callback: Error in set opt to session ${req.sessionID}`)
            return res.status(500).json({ message: "internal server error" })
        }
        return res.json({ message: "ok", otpCode: otpCode })
    }) 
}

async function getSession(req, res) { 
    const otpCode = req.body?.code

    if(!uuidValidate(otpCode)) 
        return res.status(400).json({ message: "invalid field @code" })

    redis.multi()
    .get(`otp-${otpCode}`)
    .del(`otp-${otpCode}`)
    .exec((err, results) => { 

        if(err){
            console.error(`at /session/login/get-session: Error in get opt session ${otpCode}`)
            return res.status(500).json({ message: "internal server error" })
        }
        
        const sessionID = results[0][1]
        
        if(sessionID === null)
            return res.status(400).json({ message: "otp code expired!" })
        
        req.sessionID = sessionID
        req.sessionStore.get(sessionID, function (err, session) {
            if(err || session === undefined)
                return res.status(400).json({ message: "original session expired!" })

            req.sessionStore.createSession(req, session);
            return res.json({ message: "ok" })
        })
    })
}

async function logout(req, res) { 
    req.logOut() 
    return res.json({ message: "ok" })
}