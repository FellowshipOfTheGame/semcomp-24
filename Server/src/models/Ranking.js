const mongoose = require('mongoose');

const RankingSchema = new mongoose.Schema({
    
    rank: [{
        userId: {
            type: mongoose.Schema.Types.ObjectId,
            ref: 'User',
        },

        nickname: { 
            type: String,
            required: true
        },

        score: { 
            type: Number,
            required: true
        },
    }],
    
    minScore: { 
        type: Number,
        required: true,
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

module.exports = mongoose.model('Ranking', RankingSchema);