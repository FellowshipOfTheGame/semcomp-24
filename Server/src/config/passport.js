let GoogleStrategy = require('passport-google-oauth20').Strategy;
let UserController = require('../controllers/userController');
let User = require('../models/User');

require('dotenv').config()

const SERVER_HTTP_PORT = process.env.SERVER_HTTP_PORT || 3000
const SERVER_HTTPS_PORT = process.env.SERVER_HTTPS_PORT || undefined

module.exports = function (passport) {
    passport.serializeUser(function(user, done){
        done(null, {
            id: user._id,
        });
        // done(null, user);
    });

    passport.deserializeUser(function(obj, done){
        User.findById(obj.id, function(err,user){
            done(err, user);    
        });
        // done(null, obj);
    });

    passport.use(new GoogleStrategy({
        clientID: process.env.GOOGLE_CLIENT_ID,
        clientSecret: process.env.GOOGLE_CLIENT_SECRET,
        callbackURL: `http${SERVER_HTTPS_PORT ? 's' : ''}://localhost:${SERVER_HTTPS_PORT ? SERVER_HTTPS_PORT : SERVER_HTTP_PORT}/session/login/callback`
    },
        function(accessToken, refreshToken, profile, done) {
            UserController.findOrCreate(profile, (err, user) => {
                if (err) {
                    console.log(err);
                    return done(err, null, { message: "Não foi possível criar ou encontrar usuário" })
                }

                if (!user) {
                    return done(null, null, { message: "Usuário não criado ou não encontrado" });
                }
                
                return done(null, user);
            });
        }
    ));
}