const express = require("express");

const port = process.env.port || 1447;

const app = express();
app.use(express.json());

var interactions = [];
var conversations = [];
function collectRequestData(request) {
    let interaction = {
        path: request.path,
        body: request.body
    };

    interactions.push(
        interaction
    );
    console.log(interaction);
}

app.get("/interactions/next", (req, res) => {

    if (interactions.length === 0) {
        return;
    }

    var interaction = res.json(interactions[0]);
    interactions.shift();

    return interaction;
});

app.post("/interactions/reset", (req, res) => {
    interactions = [];
    conversations = [];
    res.sendStatus(200)
});


app.get("/api/usergroups.list", (req, res) => {
    collectRequestData(req);

    return res.json({
        "Ok": true
    });
});



app.post("/api/channels.create", async (req, res) => {
    collectRequestData(req);

    let channel = {
        "Id": generateRandomString(9).toUpperCase(),
        "Name": req.body.name
    };
    conversations.push(channel);

    return res.json({
        "Ok": true,
        "Channel": channel
    });
});

app.post("/api/usergroups.create", async (req, res) => {
    collectRequestData(req);

    return res.json({
        "Ok": true,
        "UserGroup": { "Id": "id", "Name": "name", "Handle": "handle" }
    });
});

app.post("/api/chat.postMessage", async (req, res) => {
    collectRequestData(req);

    return res.json({
        "Ok": true,
        "ts": "1355517523.000005"
    });
});

app.post("/api/pins.add", async (req, res) => {
    collectRequestData(req);

    return res.json({
        "Ok": true
    });
});

app.post("/api/channels.join", async (req, res) => {
    collectRequestData(req);
    
    let channel = conversations
    .find(function (conversation) { return (conversation.name == req.body.name); });
    
    if(channel === undefined){ 
        channel = {
            "Id": generateRandomString(9).toUpperCase(),
            "Name": req.body.name
        };
        conversations.push(channel);
    }

    return res.json({
        "Ok": true,
        "Channel": channel
    });
});

app.get("/api/conversations.list", (req, res) => {
    collectRequestData(req);

    return res.json({
        "Ok": true,
        "Channels" : conversations
    });
});


app.get("/api/usergroups.list", (req, res) => {
    collectRequestData(req);

    return res.json({
        "Ok": true
    });
});



function generateRandomString(length) {
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < length; i++) {
        result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    return result;
}


app.listen(port, () => {
    console.log("Slack API spy is listening on port " + port);
});
