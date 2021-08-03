const dotenv = require("dotenv")

dotenv.config();

module.exports = {
    NODE_ENV: process.env.NODE_ENV || "dev",
    ENABLE_HTTP: process.env.ENABLE_HTTP == "1" || false,
    ENABLE_HTTPS: process.env.ENABLE_HTTP == "1" || false,
    
    SERVER_HTTP_PORT: process.env.SERVER_HTTP_PORT || 3000,
    SERVER_HTTPS_PORT: process.env.SERVER_HTTPS_PORT || undefined,
    
    CERTIFICATE_KEY_PATH: process.env.CERTIFICATE_KEY_PATH || undefined,
    CERTIFICATE_CERT_PATH: process.env.CERTIFICATE_CERT_PATH || undefined,
    CERTIFICATE_CA_PATH: process.env.CERTIFICATE_CA_PATH || undefined,
    
    REDIS_HOST: process.env.REDIS_HOST || "localhost",
    REDIS_PORT: process.env.REDIS_PORT ||  6379,

    MONGO_CONNECT_URL: process.env.REDIS_HOST.MONGO_CONNECT_URL || "mongodb://localhost:27017/myapp",

    GOOGLE_CLIENT_ID: process.env.GOOGLE_CLIENT_ID || undefined,
    GOOGLE_CLIENT_SECRET: process.env.GOOGLE_CLIENT_SECRET|| undefined,

    REQUEST_SIGNATURE_KEY: process.env.REQUEST_SIGNATURE_KEY || "MINAHSENHASUPERSECRETA",
}