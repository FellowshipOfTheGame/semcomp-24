const mongoose = require('mongoose');

const UpgradesModel = require("../models/UserUpgrades")

const UserSchema = new mongoose.Schema({
    created_at: {
        type: Date,
        required: true,
    },
    
    provider: {
        type: String,
        required: true,
    },
    provider_id: {
        type: String,
        required: true,
    },
    name: {
        type: String,
        required: true,
    },
    email: {
        type: String,
        required: true,
        unique: true,
    },

    gold: {
        type: Number,
        default: 0,
        min: 0,
    },
    goldAcc: { 
        type: Number,
        default: 0,
        min: 0,
    },
    runs: {
        type: Number,
        default: 0,
    },
    upgrades: {
        type: [UpgradesModel],
        default:[{
            itemName: "Max_Life",
        }, {
            itemName: "Booster",
        }, {
            itemName: "Nitro",
        }, {
            itemName: "Bus_Stop",
        }, {
            itemName: "Shield",
        }, {
            itemName: "Laser",
        }]
    },
    topScore: {
        type: Number,
        default: 0,
    },
    topScoreDate: {
        type: Date,
        required: true,
        default: Date.now
    },

    isBanned: {
        type: Boolean,
        required: true,
        default: false,
    },
});

// Add custom index to optimize Ranking Loading
UserSchema.index({topScore: -1, topScoreDate: 1})

module.exports = mongoose.model('User', UserSchema);