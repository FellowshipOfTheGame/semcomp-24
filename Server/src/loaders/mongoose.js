// Dependencies
const mongoose = require('mongoose');
const config = require("../config");

// Database Singleton (using Mongoose)
class Database {

    constructor() {
        this._connect()
    }

    _connect() {
        mongoose.connect(config.MONGO_CONNECT_URL)
        .then(() => {
            console.log(`MongoDB at ${config.MONGO_CONNECT_URL} connect successful`)
        })
        .catch(err => {
            console.error(`MongoDB at ${config.MONGO_CONNECT_URL} connection error`)
            console.error(err)
        })
    }
}

module.exports = new Database()