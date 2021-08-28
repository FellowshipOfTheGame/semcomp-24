// Dependencies
const mongoose = require('mongoose');
const config = require("../config");

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
            console.log(`at MongoDB: ${config.MONGO_CONNECT_URL} connect successful`)
        })
        .catch(err => {
            console.error(`at MongoDB: ${err}`)
            console.error(err)
        })
    }
}

module.exports = new Database()