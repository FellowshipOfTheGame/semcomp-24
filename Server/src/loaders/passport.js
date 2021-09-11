let GoogleStrategy = require('passport-google-oauth20').Strategy;

const config = require('../config')
const User = require('../models/User');
const UserController = require('../controllers/userController');

const { logger } = require('../config/logger');

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
            clientID: config.GOOGLE_CLIENT_ID,
            clientSecret: config.GOOGLE_CLIENT_SECRET,
            callbackURL: config.GOOGLE_CALLBACK_URL,
        },
        function(accessToken, refreshToken, profile, done) {
            UserController.findOrCreate(profile, (err, user) => {
                if (err) {
                    logger.error({
                        message: `at Google Login: ${err}`
                    })
                    return done(err, null, { message: "unable to create or find user" })
                }

                if (!user) {
                    return done(null, null, { message: "user not created or not found" });
                }
                
                return done(null, user);
            });
        }
    ));
}