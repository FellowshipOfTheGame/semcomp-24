// Dependencies
const express = require('express');
const routes = express.Router();

// Routes
routes.post('/register', (req, res) => res.json({ serverStatus: "OK" }))
routes.get('/status', (req, res) => res.json({ serverStatus: "OK" }))

// Export routes
module.exports = routes;