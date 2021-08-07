// Dependencies

const UserModel = require("../models/User")
const ShopModel = require("../models/Shop")

// Exporting controller async functions
module.exports = { 
    shop,
    buy
}

// Controller Functions

// shop function: retrieves upgrades from shop, retrieves and displays the amount of gold user has
// GET request
async function shop(req, res) {
    const userId = req.user._id  //string

    var user = await UserModel.findById(userId)
    var userGold = user.gold
    const userUpgrades = await user.upgrades

    // makes copy to avoid referencing the original
    var shopUpgrades = JSON.parse(JSON.stringify(userUpgrades))

    // if user already has an upgrade maximized, do not display it
    shopUpgrades = shopUpgrades.filter(function(obj) {
        obj.level++ // add +1 to level and multiply price
        obj.price = obj.level * obj.price
        return (obj.level < 4);
    })

    // else display it as usual
    return res.status(200).json({ message: "ok", gold: userGold, shop: shopUpgrades })
}

// buy function: exchanges user's gold for the upgrade they want to buy
// POST request
async function buy(req, res) { 
    const userId = req.user._id  //string

    const upgradeName = req.body?.upgrade_name.toString()   //string

    var user = await UserModel.findById(userId)
    var userGold = user.gold
    var userUpgrades = await user.upgrades

    // current upgrade values
    var currUpgrade = userUpgrades.find(obj => obj.itemName === upgradeName)

    if(!currUpgrade)
        return res.status(400).json({ message: "Upgrade not found" })

    // target upgrade values
    // makes copy to avoid referencing the original
    var targetUpgrade = JSON.parse(JSON.stringify(currUpgrade))

    // add +1 to level and multiply price
    targetUpgrade.level++
    targetUpgrade.price = targetUpgrade.level * targetUpgrade.price

    const upgradeLevel = targetUpgrade.level
    const upgradePrice = targetUpgrade.price

    // check if user has already maximized the upgrade 
    if(upgradeLevel > 3)
        return res.status(400).json({ message: "User already maximized this upgrade!" })
    
    // check if user has enough gold for the transaction
    if(upgradePrice > userGold)
        return res.status(400).json({ message: "User doesn't have enough gold!" })

    // if both are true, subtract user's gold and add upgrade to his "acquired upgrades"
    userGold -= upgradePrice
    currUpgrade.level++

    // add change to database
    user.gold = userGold
    await user.save()

    return res.status(200).json({ message: "ok", userGold: userGold, upgrade: upgradeName, upgrade_level: upgradeLevel, upgrade_price: upgradePrice })
}