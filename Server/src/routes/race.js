// Dependencies
const routes = require('express').Router();

// Middlewares
const SessionMiddleware = require('../middlewares/Session.middleware');

// Controllers
const raceController = require('../controllers/raceController')

// Routes
routes.post('/start', SessionMiddleware.isAuth, raceController.start)
routes.post('/finish', SessionMiddleware.isAuth, raceController.finish)
routes.get('/ranking', raceController.ranking)

// Export routes
module.exports = routes;