const mongoose = require('mongoose');

const RaceSchema = new mongoose.Schema({
    userId: {
        type: mongoose.Schema.Types.ObjectId,
        ref: 'User',
        // type: String,
        // required: true,
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

module.exports = mongoose.model('Race', RaceSchema);