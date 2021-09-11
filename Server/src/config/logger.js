const winston = require('winston');
require('winston-mongodb');

const config = require('./index')

const format = winston.format.printf(({ level, message, label }) => {
    return `[${level}] [${new Date().toUTCString()}] ${message}`;
});

const logger = winston.createLogger({
    transports: [
        new winston.transports.Console({
            format: winston.format.colorize()
        }),
        new winston.transports.File({
            filename: 'error.log',
            level: 'error'
        }),
        new winston.transports.File({
            filename: 'info.log',
            level: 'info'
        }),
        new winston.transports.MongoDB({
            level: 'info',
            db: config.MONGO_CONNECT_URL,
            collection: 'logs',
            format: winston.format.combine(
                winston.format.json(),
                format
            ),
            useUnifiedTopology: true
        })
    ],
    format: winston.format.combine(
        // winston.format.colorize(),
        winston.format.simple(),
        format
    ),
});

const raceLogger = winston.createLogger({
    transports: [
        new winston.transports.Console({
            format: winston.format.colorize()
        }),
        new winston.transports.File({
            filename: 'race.log',
            level: 'info'
        }),
        new winston.transports.MongoDB({
            level: 'info',
            db: config.MONGO_CONNECT_URL,
            collection: 'race_logs',
            format: winston.format.combine(
                winston.format.json(),
                format
            ),
            useUnifiedTopology: true
        })
    ],
    format: winston.format.combine(
        // winston.format.colorize(),
        winston.format.simple(),
        format
    ),
});

module.exports = {
    logger,
    raceLogger,
}