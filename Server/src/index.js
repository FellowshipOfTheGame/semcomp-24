// Dependencies
const express = require('express');
const https = require('https');

const userRoutes = require('./routes/user')
const sessionRoutes = require('./routes/session')
const raceRoutes = require('./routes/race')
const shopRoutes = require('./routes/shop')

// Enviroments Variables
const config = require("./config/")

// Server Configurations & Middlewares
var app = express();

app.use(express.json())

// Security and Log Configurations
// TODO (https://expressjs.com/pt-br/advanced/best-practice-security.html)
app.disable('x-powered-by')

// Routes Configurations
app.get('/ping', (req, res) => res.json({ response: "pong :)" }))
app.use('/user', userRoutes)
app.use('/session', sessionRoutes)
app.use('/race', raceRoutes)
app.use('/shop', shopRoutes)

// Server Listeners
if(config.NODE_ENV === "dev" || config.SERVER_HTTPS_PORT === undefined){
    
    app.listen(config.SERVER_HTTP_PORT, (error) => {
        if (error) throw error
        console.log(`Starting HTTP server on port ${config.SERVER_HTTP_PORT}.`) 
    })

} else { 
    var httpsCredentials = {
        key:  fs.readFileSync(config.CERTIFICATE_KEY_PATH),
        cert: fs.readFileSync(config.CERTIFICATE_CERT_PATH),
        ca:   fs.readFileSync(config.CERTIFICATE_CA_PATH)
    }

    https.createServer(httpsCredentials, app).listen(config.SERVER_HTTPS_PORT , (error) => {
        if (error) throw error
        console.log(`Starting HTTPS server on port ${config.SERVER_HTTPS_PORT}.`) 
    })
}
