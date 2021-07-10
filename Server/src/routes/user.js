// Dependencies
const express = require('express');
const routes = express.Router();
const UserController = require('../controllers/userController');

// Routes
routes.post('/register', (req, res) => res.json({ serverStatus: "OK" }))
routes.get('/status', (req, res) => res.json({ serverStatus: "OK" }))
routes.get('/show', UserController.show);

// Export routes
module.exports = routes;