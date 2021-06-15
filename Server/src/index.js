// Dependencies and Configurations
const express = require('express');
const https = require('https');
require('dotenv').config()

// Enviroments Variables
const NODE_ENV = process.env.NODE_ENV || "dev"
const SERVER_HTTP_PORT = process.env.SERVER_HTTP_PORT || 3000
const SERVER_HTTPS_PORT = process.env.SERVER_HTTPS_PORT || undefined
const CERTIFICATE_KEY_PATH = process.env.CERTIFICATE_KEY_PATH || undefined
const CERTIFICATE_CERT_PATH = process.env.CERTIFICATE_CERT_PATH || undefined
const CERTIFICATE_CA_PATH = process.env.CERTIFICATE_CA_PATH || undefined

// Server Configurations
var app = express();

// Security and Log Configurations
// TODO (https://expressjs.com/pt-br/advanced/best-practice-security.html)
app.disable('x-powered-by')

// Routes Configurations
app.get('/', (req, res) => res.json({ serverStatus: "OK" }))

// Server Listeners
if(NODE_ENV === "dev" || SERVER_HTTPS_PORT === undefined){
    app.listen(SERVER_HTTP_PORT, (error) => {
        if (error) throw error
        console.log(`Starting HTTP server on port ${SERVER_HTTP_PORT}.`) 
    })

} else { 
    var httpsCredentials = {
        key:  fs.readFileSync(CERTIFICATE_KEY_PATH),
        cert: fs.readFileSync(CERTIFICATE_CERT_PATH),
        ca:   fs.readFileSync(CERTIFICATE_CA_PATH)
    }

    https.createServer(httpsCredentials, app).listen(SERVER_HTTPS_PORT , (error) => {
        if (error) throw error
        console.log(`Starting HTTPS server on port ${SERVER_HTTPS_PORT}.`) 
    })
}
