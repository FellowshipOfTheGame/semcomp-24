// Dependencies
const routes = require('express').Router();
const ejs    = require('ejs')
const path   = require('path')

const config = require('../config')

const templates = path.join(__dirname, '../templates')
const publicUrl = `${config.SERVER_PATH_PREFIX}`


// Views Routes
routes.get('/', async (req, res) => { 
    ejs.renderFile(`${templates}/index.html`, 
        {
            title: 'Home',
            main:  'home.html',
            public: publicUrl,
        },
        function(err, html) { 
        return res.send(html)
    })
})

routes.get('/informacoes', async (req, res) => { 
    ejs.renderFile(`${templates}/index.html`, 
        {
            title: 'Políticas & Termos',
            main:  'about.html',
            public: publicUrl,
        },
        function(err, html) { 
        return res.send(html)
    })
})




// routes.post('/loginCode', SessionMiddleware.isAuth, raceController.finish)
// routes.get('/politicas-e-termos', raceController.ranking)

// Export routes
module.exports = routes;