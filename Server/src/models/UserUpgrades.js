const mongoose = require('mongoose');

const UpgradesSchema = new mongoose.Schema({
    
    item: [{

        itemName: { 
            type: String,
            required: true
        },

        level: {
            type: Number,
            required: true,
            default: 0
        },

    }],
   
    updatedAt: { 
        type: Date,
        required: true,
        default: Date.now
    },

    createdAt: { 
        type: Date,
        required: true,
        default: Date.now
    },
})

module.exports = mongoose.model('UserUpgrades', UpgradesSchema);