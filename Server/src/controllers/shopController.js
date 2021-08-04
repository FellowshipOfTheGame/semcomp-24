// Dependencies

const UserModel = require("../models/User")
const ShopModel = require("../models/Shop")

// Exporting controller async functions
module.exports = { 
    shop,
    buy
}

// Controller Functions
//create shop display for user based on his acquired upgrades
async function createShop(userUpgrades) {
    const names = ["Max Life", "Base Acceleration", "Traction", "Booster", "Nitro", "Bus Stop"]
    var shopUpgrades = []
    
    for(let i = 0; i < names.length; i++) {

        item = await ShopModel.create({
            itemName: names[i],
            level: 0,
            price: 100,
            created_at: new Date(),
            updated_at: new Date(),
        });

        shopUpgrades.push(item);
    }

    return shopUpgrades
}

//shop function: retrieves upgrades from shop, retrieves and displays the amount of gold user has
//GET request
async function shop(req, res) {
    const userId = req.query?.user_id  //string
    const userGold = 500   //integer
    const userUpgrades = req.body?.upgrades   //list

    // Shop model
    const shopUpgrades = await createShop(userUpgrades)

    //TODO: if user already has an upgrade, do not display it / have it "unavailable"
    //else display it as usual
    return res.status(200).json({ message: "ok", gold: userGold, shop: shopUpgrades })
}

//buy function: exchanges user's money for the upgrade they want to buy
//POST request
async function buy(req, res) { 
    const userId = "60f32e0105e0c858b8746d75"  //string
    const userGold = parseInt(req.body?.gold)   //integer
    const userUpgrades = req.body?.upgrades   //list
    const targetUpgrade = req.body?.upgrade.toString()   //string

    //TODO: add the real upgrade names, add mongoose model
    const shopUpgrades = {
        "thing 1": 100,
        "thing 2": 200,
        "thing 3": 300
    }  //dict of all upgrades on shop and their "prices"

    //check if user has already purchased the upgrade
    if(targetUpgrade in userUpgrades)
        return res.status(400).json({ message: "User already purchased this upgrade!" })
    
    //check if user has enough gold for the transaction
    if(shopUpgrades[targetUpgrade] > userGold)
        return res.status(400).json({ message: "User doesn't have enough gold!" })

    //if both are true, subtract user's gold and add upgrade to his "acquired upgrades"
    userGold = userGold - shopUpgrades[targetUpgrade]
    userUpgrades.push(targetUpgrade)

    //TODO: add change to database

    return res.status(200).json({ message: "ok" })
}