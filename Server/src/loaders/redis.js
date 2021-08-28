const session = require('express-session')
const redis = require('redis')
const IORedis = require("ioredis")
const redisStore = require('connect-redis')(session)

const config = require("../config")

// Redis Database 1 - Sessions Storages
const sessionClient = redis.createClient({
    db: 1,
    host: config.REDIS_HOST,
    port: config.REDIS_PORT
})

// Redis Database 2 - OTP Codes Storages
const otpClient = new IORedis({ 
    db: 2, 
    host: config.REDIS_HOST,
    port: config.REDIS_PORT, 
})

// Redis Database 3 - Nonces Client
const nonceClient = new IORedis({ 
    db: 3, 
    host: config.REDIS_HOST,
    port: config.REDIS_PORT, 
})

// Redis Clients Logs
const clientList = [ sessionClient, otpClient, nonceClient ]

clientList.forEach(client => { 
    client.on('connect', () => {
        console.log(`at Redis[${client.options.db}]: Frequency connected!`)
    })
    
    client.on('error', (err) => {
        console.error(`at Redis[${client.options.db}]: ${err}`)
    })
})

// Module Exports
module.exports = {
    sessionClient,
    sessionStore: new redisStore({ client: sessionClient, ttl: 3600 }),
    otpClient,
    nonceClient,
}