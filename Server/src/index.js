// Dependencies
const express = require('express');
const https = require('https');
const passport = require('passport');
const session = require('express-session');
const mongoose = require('mongoose');
const redis = require('./config/redis');
const cookieParser = require('cookie-parser');

const userRoutes = require('./routes/user')
const sessionRoutes = require('./routes/session')
const raceRoutes = require('./routes/race')
const shopRoutes = require('./routes/shop')

// Enviroments Variables
const config = require("./config/");
require('dotenv').config();

// Server Configurations & Middlewares
var app = express();

app.use(express.json())

// Security and Log Configurations
// TODO (https://expressjs.com/pt-br/advanced/best-practice-security.html)
app.disable('x-powered-by');

app.use(cookieParser(`${process.env.REDIS_SECRET}`));

app.use(session({
    resave: false,
    name: "semcompSession",
    saveUninitialized: false,
    cookie: {
        secure: false, 
        httpOnly: false, 
        sameSite: 'strict', 
        maxAge: 3600000 
    }, //TODO: change secure to true
    secret: `${process.env.REDIS_SECRET}`,
    store: redis.sessionStore,
}));

app.use(passport.initialize());
app.use(passport.session());

try {
    mongoose.connect(`mongodb://localhost:27017/semcomp-24`, {
        useNewUrlParser: true,
        useUnifiedTopology: true,
    });
    console.log("MongoDB Connected");    
} catch (error) {
    console.log("MongoDB Error");
    console.log(error);
}

app.use((req, res, next) => {
    console.log(req.method, req.path);
    next()
})

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
