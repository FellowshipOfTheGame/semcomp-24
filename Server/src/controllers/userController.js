const User = require('../models/User');

module.exports = {
    async findOrCreate (google_user, cb) {
        if (!google_user.id) {
            return cb({ message: "Um google_id é necessário" }, null);
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
            console.log(err);
            return res.status(500).json({ message: "Não foi possível encontrar usuário" });
        }

        if (!user) {
            return res.status(404).json({ message: "Usuário não encontrado" });
        }

        return res.status(200).end(user);
    },

    async showAll (req, res) {
        try {
            var users = await User.find({})
                                  .select("nickname photo");
        } catch (err) {
            console.log(err);
            return res.status(500).json({ message: "Não foi possível encontrar usuários" });
        }

        return res.status(200).json(users);
    }
}