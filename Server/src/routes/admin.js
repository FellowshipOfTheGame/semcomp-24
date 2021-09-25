// Dependencies
const routes = require('express').Router();

// Controllers
const config = require("../config/")
const adminController = require('../controllers/adminController')

async function checkAdminAPYKEY(req, res, next) {
    if(req.query.ADMIN_APIKEY !== config.ADMIN_APIKEY)
        return res.status(401).json({ message: "unauthorized" });
    next();
}

// Routes
routes.put('/top-races', checkAdminAPYKEY,  adminController.getTopRaces)

// Export routes
module.exports = routes;