// Dependencies

const UserModel = require("../models/User")

// Exporting controller async functions
module.exports = { 
    shop,
    buy
}

// Controller Functions

// shop function: retrieves upgrades from shop, retrieves and displays the amount of gold user has
// GET request
async function shop(req, res) {

    const userGold = req.user.gold
    const userUpgrades = req.user.upgrades

    // makes copy to avoid referencing the original
    // let shopUpgrades = JSON.parse(JSON.stringify(userUpgrades))
    let shopUpgrades = Object.assign([], userUpgrades);

    if (!shopUpgrades) {
        return res.status(404).json({ message: "upgrades not found" })
    }

    // if user already has an upgrade maximized, do not display it
    shopUpgrades = shopUpgrades.filter(function(obj) {
        obj.level++ // add +1 to level
        return (obj.level > -1);
    })
    .map((obj) => {return { itemName: obj.itemName, level: obj.level, price: obj.price }})

    // else display it as usual
    return res.status(200).json({ message: "ok", gold: userGold, shop: shopUpgrades })
}

// buy function: exchanges user's gold for the upgrade they want to buy
// POST request
async function buy(req, res) { 

    const userId = req.user._id  //string
    console.log(userId)
    const upgradeName = req.body?.itemName.toString()   //string
    let userGold = req.user.gold
    let userUpgrades = req.user.upgrades

    // current upgrade values
    let currUpgrade = userUpgrades.find(obj => obj.itemName === upgradeName)

    if(!currUpgrade)
        return res.status(404).json({ message: "upgrade not found" })

    // target upgrade values
    // makes copy to avoid referencing the original
    let targetUpgrade = JSON.parse(JSON.stringify(currUpgrade))

    // add +1 to level
    targetUpgrade.level++

    let upgradeLevel = targetUpgrade.level
    let upgradePrice = targetUpgrade.price

    // check if user has already maximized the upgrade 
    if(upgradeLevel > 3)
        return res.status(400).json({ message: "user already maximized this upgrade" })
    
    // check if user has enough gold for the transaction
    if(upgradePrice > userGold)
        return res.status(400).json({ message: "user doesn't have enough gold" })

    // if both are true, subtract user's gold and add upgrade to his "acquired upgrades"
    userGold -= upgradePrice
    upgradeLevel = upgradeLevel + 1
    upgradePrice = upgradePrice + 100

    currUpgrade.level++
    currUpgrade.price = currUpgrade.price + 100

    // add change to database
    req.user.gold = userGold

    try {
        await req.user.save()
        .then(() => { 
            return res.status(200).json({ message: "ok", gold: userGold, itemName: upgradeName, level: upgradeLevel, price: upgradePrice })
        })
    }
    catch (err) {
        console.error(`Failed to save upgrade ${upgradeName} to user ${userId}. ${err}`)
        return res.status(500).json({ message: "Internal server error" });
    }
}