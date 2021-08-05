// Dependencies

const UserModel = require("../models/User")
const ShopModel = require("../models/Shop")

// Exporting controller async functions
module.exports = { 
    shop,
    buy
}

// Controller Functions

// create shop display for user based on his acquired upgrades
async function createShop(userUpgrades) {
    const names = ["Max Life", "Base Acceleration", "Traction", "Booster", "Nitro", "Bus Stop"]
    var shopUpgrades = []
    
    for(let i = 0; i < names.length; i++) {
        const upLevel = userUpgrades[i].level + 1

        item = await ShopModel.create({
            itemName: names[i],
            level: upLevel,
            price: upLevel * 100,
            created_at: new Date(),
            updated_at: new Date(),
        });

        shopUpgrades.push(item);
    }

    return shopUpgrades
}

// shop function: retrieves upgrades from shop, retrieves and displays the amount of gold user has
// GET request
async function shop(req, res) {
    const userId = req.query?.user_id  //string

    var user = await UserModel.findById(userId)
    var userGold = user.gold
    var userUpgrades = await user.upgrades

    // Shop model
    const shopUpgrades = await createShop(userUpgrades)

    // TODO: if user already has an upgrade, do not display it / have it "unavailable"
    // else display it as usual
    return res.status(200).json({ message: "ok", gold: userGold, shop: shopUpgrades })
}

// buy function: exchanges user's gold for the upgrade they want to buy
// POST request
async function buy(req, res) { 
    const userId = req.query?.user_id  //string

    const upgradeIndex = req.body?.upgrade_index  //integer
    const upgradeName = req.body?.upgrade_name.toString()   //string
    const upgradeLevel = req.body?.upgrade_level  //integer
    const upgradePrice = req.body?.upgrade_price  //integer

    var user = await UserModel.findById(userId)
    var userGold = user.gold
    var userUpgrades = await user.upgrades

    // check if user has already purchased the upgrade
    if(upgradeLevel !== userUpgrades[upgradeIndex].level + 1)
        return res.status(400).json({ message: "User cannot purchase this upgrade!" })
    
    // check if user has enough gold for the transaction
    if(upgradePrice > userGold)
        return res.status(400).json({ message: "User doesn't have enough gold!" })

    // if both are true, subtract user's gold and add upgrade to his "acquired upgrades"
    userGold -= upgradePrice
    userUpgrades[upgradeIndex].level++

    // add change to database
    user.gold = userGold
    user.upgrades[upgradeIndex].level = userUpgrades[upgradeIndex].level
    await user.save()

    return res.status(200).json({ message: "ok", userGold: userGold, upgrade: upgradeName, upgrade_level: upgradeLevel })
}