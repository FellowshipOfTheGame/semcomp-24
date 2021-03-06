// Dependencies
const express = require('express');
const https = require('https');
const passport = require('passport');
const cors = require('cors')
const fs = require('fs');
const path = require('path')

const { logger } = require('./config/logger');

// Singletons & Libraries Loaders
require('./loaders/mongoose')
require('./loaders/redis')
const session = require('./loaders/session')
require('./loaders/passport')(passport)

// Routes
const userRoutes = require('./routes/user')
const sessionRoutes = require('./routes/session')
const raceRoutes = require('./routes/race')
const shopRoutes = require('./routes/shop')
const viewsRoutes = require('./routes/views')
const adminRoutes = require('./routes/admin')

// Enviroments Variables
const config = require("./config/");

// Server Configurations & Middlewares
var app = express();

app.set('trust proxy', true)
app.use(express.json())
app.use(session.cookieLoader())
app.use(session.sessionLoader())
app.use(passport.initialize());
app.use(passport.session());
app.use(config.SERVER_PATH_PREFIX, express.static(path.join(__dirname, 'public')));

// Enable cors to all origins (because we are an API after all :P)
app.use(cors({
    credentials: true,
    origin: /^https:\/\/[a-zA-Z0-9]*\.ssl\.hwcdn\.net$/,
    optionsSuccessStatus: 200, // some legacy browsers (IE11, various SmartTVs) choke on 204
    "methods": "GET,HEAD,PUT,PATCH,POST,DELETE",
    "preflightContinue": false,
    exposedHeaders: ["set-cookie"],
}))

// Security and Log Configurations
// TODO (https://expressjs.com/pt-br/advanced/best-practice-security.html)
app.disable('x-powered-by');

app.use((req, res, next) => {
    let ip = req.headers['x-forwarded-for'] || (req.socket.remoteAddress);
    ip = ip.indexOf('.') >= 0 ? ip.substring(ip.lastIndexOf(':') + 1) : ip;

    req.ip = ip;

    logger.info({
        message: `${req.method} ${req.path} - User-Agent: ${req.get('User-Agent')} - ${req.ip} `
    });
    next();
})

// Routes Configurations

app.get(`${config.SERVER_PATH_PREFIX}/ping`, (req, res) => res.json({ message: "pong :)" }))
app.use(`${config.SERVER_PATH_PREFIX}/user`, userRoutes)
app.use(`${config.SERVER_PATH_PREFIX}/session`, sessionRoutes)
app.use(`${config.SERVER_PATH_PREFIX}/race`, raceRoutes)
app.use(`${config.SERVER_PATH_PREFIX}/shop`, shopRoutes)
app.use(`${config.SERVER_PATH_PREFIX}/admin`, adminRoutes)
app.use(`${config.SERVER_PATH_PREFIX}`, viewsRoutes)

// Server Listeners
if(config.ENABLE_HTTPS) { 

    var httpsCredentials = {
        key:  config.CERTIFICATE_KEY_PATH && fs.readFileSync(config.CERTIFICATE_KEY_PATH),
        cert: config.CERTIFICATE_CERT_PATH && fs.readFileSync(config.CERTIFICATE_CERT_PATH),
        ca:   config.CERTIFICATE_CA_PATH && fs.readFileSync(config.CERTIFICATE_CA_PATH)
    }

    https.createServer(httpsCredentials, app).listen(config.SERVER_PORT , (error) => {
        if (error) throw error
        logger.info({
            message: `Starting HTTPS server on port ${config.SERVER_PORT}.`
        }) 
    })

} else {

    app.listen(config.SERVER_PORT, (error) => {
        if (error) throw error
        logger.info({
            message: `Starting HTTP server on port ${config.SERVER_PORT}.`
        }) 
    })

}
