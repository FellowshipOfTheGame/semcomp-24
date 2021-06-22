// Dependencies
const express = require('express');
const routes = express.Router();

// Routes
routes.post('/login', (req, res) => res.json({ serverStatus: "OK" }))
routes.post('/logout', (req, res) => res.json({ serverStatus: "OK" }))

// Export routes
module.exports = routes;