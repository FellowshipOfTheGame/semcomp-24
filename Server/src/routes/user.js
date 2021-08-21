// Dependencies
const express = require('express');
const routes = express.Router();

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');

// Controllers
const UserController = require('../controllers/userController');

// Routes
routes.get('/status', SessionMiddleware.isAuth, UserController.getInfoWithSession)

// Export routes
module.exports = routes;