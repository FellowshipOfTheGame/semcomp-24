// Dependencies
const mongoose = require('mongoose');
const config = require("../config");
const { logger } = require("../config/logger");

// Database Singleton (using Mongoose)
class Database {

    constructor() {
        this._connect()
    }

    _connect() {
        mongoose.connect(config.MONGO_CONNECT_URL,{
            useNewUrlParser: true,
            useUnifiedTopology: true,
            useCreateIndex: true,
        })
        .then(() => {
            logger.info({
                message: `at MongoDB: ${config.MONGO_CONNECT_URL} connect successful`
            })
        })
        .catch(err => {
            logger.error({
                message: `at MongoDB: ${err}`
            })
        })
    }
}

module.exports = new Database()