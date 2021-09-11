const winston = require('winston');

const config = require('./index')

const format = winston.format.printf(({ level, message, label }) => {
    return `[${level}] [${new Date().toUTCString()}] ${message}`;
});

const logger = winston.createLogger({
    transports: [
        new winston.transports.Console(),
        new winston.transports.File({
            filename: 'error.log',
            level: 'error'
        }),
        new winston.transports.File({
            filename: 'info.log',
            level: 'info'
        }),
    ],
    meta: true,
    format: winston.format.combine(
        // winston.format.colorize(),
        winston.format.simple(),
        format
    ),
});

const raceLogger = winston.createLogger({
    transports: [
        new winston.transports.Console(),
        new winston.transports.File({
            filename: 'race.log',
            level: 'info'
        }),
    ],
    meta: true,
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