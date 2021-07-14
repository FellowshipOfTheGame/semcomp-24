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
//GET request
async function shop(req, res) {
    const userId = "user1"  //string
    const userMoney = req.body?.money   //integer
    const userItems = req.body?.items   //list
    const items = { "thing 1": 100, "thing 2": 200, "thing 3": 300 }  //dict of all items on shop and their "prices"

    //TODO: if user already has an item, do not display it / have it "unavailable"
    //else display it as usual

    return res.json({ serverStatus: "OK", endpoint: "shop" })
}

//buy function: exchanges user's money for the item they want to buy
//POST request
async function buy(req, res) { 

    const userId = "user1"  //string
    const userMoney = req.body?.money   //integer
    const userItems = req.body?.items   //list
    const target_item = req.body?.item.toString()   //string

    //TODO: check if user has already purchased the item
    //TODO: check if user has enough money for the transaction
    //if both are true, subtract user's money and add item to his "acquired items"
    //else cancel transaction

    return res.json({ serverStatus: "OK", endpoint: "shop buy" })
}