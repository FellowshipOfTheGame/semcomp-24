const mongoose = require('mongoose');

const ShopSchema = new mongoose.Schema({

    itemName: { 
        type: String,
        enum: ["Max Life", "Base Acceleration", "Traction", "Booster", "Nitro", "Bus Stop"],
        required: true
    },

    level: { 
        type: Number,
        required: true,
        max: 3,
        default: 0
    },

    price: { 
        type: Number,
        required: true
    },
   
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

module.exports = mongoose.model('Shop', ShopSchema);