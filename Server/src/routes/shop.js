// Dependencies
const express = require('express');
const routes = express.Router();

// Controllers
const shopController = require('../controllers/shopController')

// Routes
routes.get('/', shopController.shop)
routes.post('/buy-upgrades', shopController.buy)

// Export routes
module.exports = routes;