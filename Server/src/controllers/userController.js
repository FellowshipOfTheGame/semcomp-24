const { createHmac } = require("crypto")

const config = require("../config/")
const User = require('../models/User');

const { logger } = require('../config/logger');

module.exports = {
    async findOrCreate (google_user, cb) {
        if (!google_user.id) {
            return cb({ message: "a google_id is required" }, null);
        }
        
        try {
            var user = await User.findOne({ google_id: google_user.id });
        } catch (err) {
            return cb(err, null);
        }

        if (user) {
            return cb(null, user);
        }

        try {
            user = await User.create({
                created_at: new Date(),

                google_id: google_user.id,
                name: google_user._json?.name,
                email: google_user._json?.email,
                picture: google_user._json?.picture,
            });
        } catch (err) {
            return cb(err, null);
        }

        logger.info({
            message: `User ${user._id} created successfully`
        });

        return cb(null, user);
    },

    async show (req, res) {
        if (!req.query?.user_id) {
            return await module.exports.showAll(req, res);
        }

        try {
            var user = await User.findById(req.query.user_id)
                                 .select("name nickname email picture");
        } catch (err) {
            logger.error({
                message: `at User.show(): failed to find user ${req.query.user_id}`
            })
            return res.status(500).json({ message: "internal server error" });
        }

        if (!user) {
            return res.status(404).json({ message: "user not found" });
        }

        return res.status(200).end(user);
    },

    async showAll (req, res) {
        try {
            var users = await User.find({})
                                  .select("nickname photo");
        } catch (err) {
            logger.error({
                message: `at User.showAll(): failed to find all users`
            })
            return res.status(500).json({ message: "internal server error" });
        }

        return res.status(200).json(users);
    },

    async getInfoWithSession(req, res) { 
        if(!req.user){ 
            return res.status(400).json({ message: "invalid user session" });
        } else { 

            let userInfo = { 
                message: "ok", 
                name: req.user.name,
                gold: req.user.gold,
                runs: req.user.runs,
                topScore: req.user.topScore,
                upgrades: req.user.upgrades.map((i) => { 
                    return { itemName: i.itemName, level: i.level } 
                }),
                sign: ""
            }
            
            userInfo.sign =  createHmac('sha256', config.RESPONSE_SIGNATURE_KEY).update(JSON.stringify(userInfo)).digest('base64')
            return res.json(userInfo)
        }
    }
}