const session = require('express-session');
const redis = require('redis');
const redisStore = require('connect-redis')(session);

const config = require('../config/index');

const sessionClient = redis.createClient({
    db: 1,
    host: config.REDIS_HOST,
    port: config.REDIS_PORT
});

sessionClient.on('connect', () => {
    console.log('Frequency connected!');
});

sessionClient.on('error', (err) => {
    console.log('Redis error: ', err);
});

module.exports = {
    sessionClient,
    sessionStore: new redisStore({ client: sessionClient, ttl: 3600 })
}