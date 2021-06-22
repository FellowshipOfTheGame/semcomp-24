// Dependencies
// None until now....

// Exporting controller async functions
module.exports = { 
    start,
    finish,
    ranking
}

// Controller Functions
async function start(req, res) { 
    return res.json({ serverStatus: "OK", endpoint: "race start" })
}

async function finish(req, res) { 
    return res.json({ serverStatus: "OK", endpoint: "race finish" })
}

async function ranking(req, res) { 
    return res.json({ serverStatus: "OK", endpoint: "race ranking" })
}