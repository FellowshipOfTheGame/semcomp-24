// Dependencies
const routes = require('express').Router();
const ejs    = require('ejs')
const path   = require('path')

const templates = path.join(__dirname, '../templates')

// Views Routes
routes.get('/', async (req, res) => { 
    ejs.renderFile(`${templates}/index.html`, 
        {
            title: 'Home'
        },
        function(err, html) { 
        return res.send(html)
    })
})



// routes.post('/loginCode', SessionMiddleware.isAuth, raceController.finish)
// routes.get('/politicas-e-termos', raceController.ranking)

// Export routes
module.exports = routes;