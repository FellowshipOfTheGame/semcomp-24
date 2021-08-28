const session = require('express-session');
const cookieParser = require('cookie-parser');

const config = require('../config')
const redis = require('../loaders/redis')

module.exports = {
    // Cookie Parser Middleware Configuration
    cookieLoader: () => cookieParser(config.COOKIE_SIGNATURE_KEY),

    // Express Session Middleware Configuration
    // Obs: HTTPS behind proxy need to activate `trust proxy` in express
    sessionLoader: () => session({
        resave: false,
        name: "gameSession",
        saveUninitialized: false,
        cookie: {
            secure: config.SESSION_SECURE, 
            httpOnly: config.SESSION_HTTP_ONLY, 
            sameSite: config.SESSION_SAME_SITE, 
            maxAge: config.SESSION_MAX_AGE 
        },
        secret: config.COOKIE_SIGNATURE_KEY,
        store: redis.sessionStore,
    }),
}