// Dependencies
const express = require('express');
const routes = express.Router();

// Routes
routes.get('/', (req, res) => res.json({ serverStatus: "OK" }))
routes.post('/buy-upgrades', (req, res) => res.json({ serverStatus: "OK" }))

// Export routes
module.exports = routes;