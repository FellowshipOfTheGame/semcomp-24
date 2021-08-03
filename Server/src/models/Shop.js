const mongoose = require('mongoose');

const ShopSchema = new mongoose.Schema({
    
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

        price: { 
            type: Number,
            required: true
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

module.exports = mongoose.model('Shop', ShopSchema);