// Dependencies
const Redis = require("ioredis");


const redis = new Redis({ port: config.REDIS_PORT, host: config.REDIS_HOST}); 

// Exporting controller async functions
module.exports = { 
    shop,
    buy
}

// Controller Functions

//shop function: retrieves items from shop, retrieves and displays the amount of money user has
async function shop(req, res) {
    const userId = "user1"  //string
    const userMoney = req.body?.money   //integer
    const items = []  //dict of all items on shop and their "prices"

    return res.json({ serverStatus: "OK", endpoint: "shop" })
}

//buy function: exchanges user's money for the item they want to buy
async function buy(req, res) { 

    const userId = "user1"  //string
    const userMoney = req.body?.money   //integer
    const target_item = req.body?.item.toString()   //string

    return res.json({ serverStatus: "OK", endpoint: "shop buy" })
}