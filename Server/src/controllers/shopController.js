// Dependencies
const Redis = require("ioredis");
const redis = new Redis({ port: config.REDIS_PORT, host: config.REDIS_HOST}); 

const UserModel = require("../models/User")
const ShopModel = require("../models/Shop")

// Exporting controller async functions
module.exports = { 
    shop,
    buy
}

// Controller Functions

//shop function: retrieves items from shop, retrieves and displays the amount of money user has
//GET request
async function shop(req, res) {
    const userId = "60f32e0105e0c858b8746d75"  //string
    const userGold = parseInt(req.body?.gold)   //integer
    const userItems = req.body?.items   //list

    const shopItems = {
        "thing 1": 100,
        "thing 2": 200,
        "thing 3": 300
    }  //dict of all items on shop and their "prices"

    //TODO: if user already has an item, do not display it / have it "unavailable"
    //else display it as usual

    return res.json({ serverStatus: "OK", endpoint: "shop" })
}

//buy function: exchanges user's money for the item they want to buy
//POST request
async function buy(req, res) { 
    const userId = "60f32e0105e0c858b8746d75"  //string
    const userGold = parseInt(req.body?.gold)   //integer
    const userItems = req.body?.items   //list
    const targetItem = req.body?.item.toString()   //string

    const shopItems = {
        "thing 1": 100,
        "thing 2": 200,
        "thing 3": 300
    }  //dict of all items on shop and their "prices"

    //check if user has already purchased the item
    if(targetItem in userItems)
        return res.status(400).json({ message: "User already purchased this item!" })
    
    //check if user has enough gold for the transaction
    if(shopItems[targetItem] > userGold)
        return res.status(400).json({ message: "User doesn't have enough gold!" })

    //if both are true, subtract user's gold and add item to his "acquired items"
    userGold = userGold - shopItems[targetItem]
    userItems.push(targetItem)

    //TODO: add change to database

    return res.json({ message: "ok" })
}