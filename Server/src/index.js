// Dependencies
const express = require('express');
const https = require('https');
const passport = require('passport');
const fs = require('fs');

// Singletons & Libraries Loaders
require('./loaders/mongoose')
require('./loaders/redis')
const session = require('./loaders/session')

// Routes
const userRoutes = require('./routes/user')
const sessionRoutes = require('./routes/session')
const raceRoutes = require('./routes/race')
const shopRoutes = require('./routes/shop')

// Enviroments Variables
const config = require("./config/");

// Server Configurations & Middlewares
var app = express();

app.set('trust proxy', config.SERVER_TRUST_PROXY)
app.use(express.json())
app.use(session.cookieLoader())
app.use(session.sessionLoader())
app.use(passport.initialize());
app.use(passport.session());

// Security and Log Configurations
// TODO (https://expressjs.com/pt-br/advanced/best-practice-security.html)
app.disable('x-powered-by');
app.use((req, res, next) => {
    console.info(`[${new Date().toUTCString()}] ${req.method} ${req.path} - User-Agent: ${req.get('User-Agent')}`);
    next()
})

// Routes Configurations
app.get('/ping', (req, res) => res.json({ message: "pong :)" }))
app.use('/user', userRoutes)
app.use('/session', sessionRoutes)
app.use('/race', raceRoutes)
app.use('/shop', shopRoutes)

// Server Listeners
if(config.ENABLE_HTTPS) { 

    var httpsCredentials = {
        key:  config.CERTIFICATE_KEY_PATH && fs.readFileSync(config.CERTIFICATE_KEY_PATH),
        cert: config.CERTIFICATE_CERT_PATH && fs.readFileSync(config.CERTIFICATE_CERT_PATH),
        ca:   config.CERTIFICATE_CA_PATH && fs.readFileSync(config.CERTIFICATE_CA_PATH)
    }

    https.createServer(httpsCredentials, app).listen(config.SERVER_PORT , (error) => {
        if (error) throw error
        console.log(`Starting HTTPS server on port ${config.SERVER_PORT}.`) 
    })

} else {

    app.listen(config.SERVER_PORT, (error) => {
        if (error) throw error
        console.log(`Starting HTTP server on port ${config.SERVER_PORT}.`) 
    })

}
