// Dependencies
const express = require('express');
const routes = express.Router();
const passport = require('passport');
require('../config/passport')(passport);

const SessionMiddleware = require('../middlewares/Session.middleware');

// Routes
routes.get('/login', passport.authenticate('google', { scope: ['profile', 'email'] , access_type: 'online' }))
routes.get('/login/callback', passport.authenticate('google', { failureRedirect: '/session/login' }),
    function(req, res) {
        res.redirect('/');
    }
);
routes.post('/logout', (req, res) => { req.logOut(); return res.status(200).end() });
routes.get('/validate', SessionMiddleware.isAuth, (req, res) => { return res.status(200).end() })

// Export routes
module.exports = routes;