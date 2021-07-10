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
    }
});

module.exports = mongoose.model('User', UserSchema);