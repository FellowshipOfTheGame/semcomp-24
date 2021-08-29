const mongoose = require('mongoose');

const UpgradesModel = require("../models/UserUpgrades")

const UserSchema = new mongoose.Schema({
    created_at: {
        type: Date,
        required: true,
    },

    google_id: {
        type: String,
        required: true,
        unique: true,
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
    picture: {
        type: String,
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
            itemName: "Base_Acceleration",
        }, {
            itemName: "Traction",
        }, {
            itemName: "Booster",
        }, {
            itemName: "Nitro",
        }, {
            itemName: "Bus_Stop",
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
});

// Add custom index to optimize Ranking Loading
UserSchema.index({topScore: -1, topScoreDate: 1})

module.exports = mongoose.model('User', UserSchema);