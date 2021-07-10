// Dependencies
const express = require('express');
const routes = express.Router();
const passport = require('passport');
require('../config/passport')(passport);

// Routes
routes.get('/login', passport.authenticate('google', { scope: ['profile', 'email'] , access_type: 'online' }))
routes.get('/login/callback', passport.authenticate('google', { failureRedirect: '/session/login' }),
    function(req, res) {
        res.redirect('/');
    }
);
routes.post('/logout', (req, res) => res.json({ serverStatus: "OK" }))

// Export routes
module.exports = routes;