const mongoose = require('mongoose');

const RaceSchema = new mongoose.Schema({
    userId: {
        type: mongoose.Schema.Types.ObjectId,
        ref: 'User',
    },

    score: { 
        type: Number,
        required: true
    },

    gold: {
        type: Number,
        required: true
    },

    startedAt: { 
        type: Date,
        required: true
    },

    finishedAt: {
        type: Date,
        required: true
    },
})

// Add custom index to optimize Ranking Loading
RaceSchema.index({score: -1, finishedAt: -1})

module.exports = mongoose.model('Race', RaceSchema);