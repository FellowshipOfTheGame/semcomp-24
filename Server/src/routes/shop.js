// Dependencies
const express = require('express');
const routes = express.Router();

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');

// Controllers
const shopController = require('../controllers/shopController')

// Routes
routes.get('/', SessionMiddleware.isAuth, shopController.shop)
routes.post('/buy-upgrades', SessionMiddleware.isAuth, shopController.buy)

// Export routes
module.exports = routes;