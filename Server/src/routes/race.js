// Dependencies
const routes = require('express').Router();

// Middlewares
// None until now....

// Controllers
const raceController = require('../controllers/raceController')

// Routes
routes.post('/start', raceController.start)
routes.post('/finish', raceController.finish)
routes.get('/ranking', raceController.ranking)

// Export routes
module.exports = routes;