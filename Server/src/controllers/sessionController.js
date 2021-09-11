// Dependencies
const Redis = require("ioredis")
const passport = require('passport')
const { v4: uuidv4, validate: uuidValidate } = require('uuid')

const config = require('../config')
const { otpClient: redis } = require("../loaders/redis")

const { logger } = require('../config/logger');

// Exporting controller async functions
module.exports = { 
    loginCallback,
    getSession,
    logout,
}

// Controller Functions
async function loginCallback(req, res) { 
    const otpCode = uuidv4()

    redis.set(`otp-${otpCode}`, req.sessionID, "EX", 3*60, (err) => { // Expire in 3 minutes        
        if(err){
            logger.error({
                message: `at Session.loginCallback(): Error in set opt to session ${req.sessionID}`
            })
            // return res.status(500).json({ message: "internal server error" })
            return res.redirect(`${config.SERVER_PATH_PREFIX}/?auth=failed`)

        }
        // return res.json({ message: "ok", otpCode: otpCode })
        res.redirect(`${config.SERVER_PATH_PREFIX}/codigo-login?code=${otpCode}`)
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
            logger.error({
                message: `at Session.getSession: Error in get opt session ${otpCode}`
            })

            return res.status(500).json({ message: "internal server error" })
        }
        
        const sessionID = results[0][1]
        
        if(sessionID === null) {
            logger.warn({
                message: `[${req.ip}] - at Session.getSession(): Failed to retrieve session from OTP code`
            })
            return res.status(400).json({ message: "otp code expired" })
        }
        
        req.sessionID = sessionID
        req.sessionStore.get(sessionID, function (err, session) {
            if(err || session === undefined)
                return res.status(400).json({ message: "original session expired" })

            req.sessionStore.createSession(req, session);
            return res.json({ message: "ok" })
        })
    })
}

async function logout(req, res) { 
    req.logOut() 
    return res.json({ message: "ok" })
}