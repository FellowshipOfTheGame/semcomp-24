// Dependencies
const config = require("../config/")
const RaceModel = require("../models/Race")

const { logger } = require('../config/logger')

// Exporting controller async functions
module.exports = { 
    getTopRaces
}

async function getTopRaces(req, res){
    const limit = parseInt(req.query.limit || 20)
    const skip = parseInt(req.query.skip || 0)

    RaceModel.find().lean()
    .sort({score: -1, finishedAt: -1})
    .skip(skip).limit(limit)
    .populate({ path: 'userId', select: 'name email', options: { lean: true }})
    .exec()
    .then((data) => {
        return res.json(data)
    })
    .catch((err) => { 
        logger.error({
            message: `at Admin.getTopRaces(): error loading best races. Error: ${err}`
        })

        return res.status(500).json({ message: "internal server error" })
    })
}

