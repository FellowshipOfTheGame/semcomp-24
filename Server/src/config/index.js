const dotenv = require("dotenv")

dotenv.config();

module.exports = {
    NODE_ENV: process.env.NODE_ENV || "dev",
    ENABLE_HTTPS: process.env.ENABLE_HTTPS == "1" || false,
    
    SERVER_TRUST_PROXY: process.env.SERVER_TRUST_PROXY == "1" || false,
    SERVER_PORT: process.env.SERVER_PORT || 3000,
    SERVER_HOST: process.env.SERVER_HOST || 'localhost',
    SERVER_PATH_PREFIX: process.env.SERVER_PATH_PREFIX || '',
    
    CERTIFICATE_KEY_PATH: process.env.CERTIFICATE_KEY_PATH || undefined,
    CERTIFICATE_CERT_PATH: process.env.CERTIFICATE_CERT_PATH || undefined,
    CERTIFICATE_CA_PATH: process.env.CERTIFICATE_CA_PATH || undefined,
    
    REDIS_HOST: process.env.REDIS_HOST || "localhost",
    REDIS_PORT: process.env.REDIS_PORT ||  6379,

    MONGO_CONNECT_URL: process.env.MONGO_CONNECT_URL || "mongodb://localhost:27017/myapp",

    GOOGLE_CLIENT_ID: process.env.GOOGLE_CLIENT_ID || undefined,
    GOOGLE_CLIENT_SECRET: process.env.GOOGLE_CLIENT_SECRET|| undefined,
    GOOGLE_CALLBACK_URL: process.env.GOOGLE_CALLBACK_URL || "http://localhost:3000/session/login/callback",

    FACEBOOK_CLIENT_ID: process.env.FACEBOOK_CLIENT_ID || undefined,
    FACEBOOK_CLIENT_SECRET: process.env.FACEBOOK_CLIENT_SECRET|| undefined,
    FACEBOOK_CALLBACK_URL: process.env.FACEBOOK_CALLBACK_URL || "http://localhost:3000/session/login/callback",

    COOKIE_SIGNATURE_KEY: process.env.COOKIE_SIGNATURE_KEY || "MINHASENHAMEGASECRETA",
    SESSION_SECURE: process.env.SESSION_SECURE == "1" || false, 
    SESSION_HTTP_ONLY: process.env.SESSION_HTTP_ONLY == "1" || false,
    SESSION_SAME_SITE: process.env.SESSION_SAME_SITE || "strict",
    SESSION_MAX_AGE: process.env.SESSION_MAX_AGE || 3600000,
    
    REQUEST_SIGNATURE_KEY: process.env.REQUEST_SIGNATURE_KEY || "MINAHSENHASUPERSECRETA",
    RESPONSE_SIGNATURE_KEY: process.env.RESPONSE_SIGNATURE_KEY || "MINAHSENHAHYPERSECRETA",
}