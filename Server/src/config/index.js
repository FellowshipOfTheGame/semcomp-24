const dotenv = require("dotenv")

dotenv.config();

module.exports = {
    NODE_ENV: process.env.NODE_ENV || "dev",
    
    SERVER_HTTP_PORT: process.env.SERVER_HTTP_PORT || 3000,
    SERVER_HTTPS_PORT: process.env.SERVER_HTTPS_PORT || undefined,
    
    CERTIFICATE_KEY_PATH: process.env.CERTIFICATE_KEY_PATH || undefined,
    CERTIFICATE_CERT_PATH: process.env.CERTIFICATE_CERT_PATH || undefined,
    CERTIFICATE_CA_PATH: process.env.CERTIFICATE_CA_PATH || undefined,

    REDIS_HOST: process.env.REDIS_HOST || "localhost",
    REDIS_PORT: process.env.REDIS_PORT ||  6379,
}