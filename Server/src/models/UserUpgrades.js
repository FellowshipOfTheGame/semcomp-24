const mongoose = require('mongoose');

const UpgradesSchema = new mongoose.Schema({
    
    itemName: { 
        type: String,
        required: true,
        enum: ["Max_Life", "Booster", "Nitro", "Bus_Stop", "Shield", "Laser"]
    },

    level: {
        type: Number,
        required: true,
        max: 3,
        default: 0
    },

    price: {
        type: Number,
        required: true,
        default: 100
    }
})

module.exports = UpgradesSchema;