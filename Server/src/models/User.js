const mongoose = require('mongoose');

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
    nickname: {
        type: String,
        // unique: true,
        maxlength: 20,
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
    },
    runs: {
        type: Number,
        default: 0,
    },
    upgrades: [{
        type: Number,
    }],
    topScores: [{
        type: Number,
    }]
});

module.exports = mongoose.model('User', UserSchema);