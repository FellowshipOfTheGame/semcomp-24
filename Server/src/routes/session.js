// Dependencies
const express = require('express');
const routes = express.Router();
const passport = require('passport');

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');

// Controllers
const SessionController = require('../controllers/sessionController');

// Routes
routes.get('/login', passport.authenticate('google', { scope: ['profile', 'email'], access_type: 'online' }))
routes.get('/login/callback', passport.authenticate('google', { failureRedirect: '/session/login' }), SessionController.loginCallback)
routes.post('/login/get-session', SessionController.getSession)

routes.get('/validate', SessionMiddleware.isAuth, (req, res) => res.json({ message: "ok" }))
routes.post('/logout', SessionController.logout);

// Export routes
module.exports = routes;