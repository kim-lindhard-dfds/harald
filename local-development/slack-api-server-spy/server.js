const express = require("express");

const port = process.env.port || 1447;

const app = express();
app.use(express.json());


app.post("/api/channels.create", async (req, res) => {
    const raw_req = req.body;
    
    console.log(raw_req);    
    return res.json({
        "Ok": true, 
        "Channel": {"Id":"id", "Name": "name"}
    });
});

app.post("/api/usergroups.create", async (req, res) => {
    const raw_req = req.body;
    
    console.log(raw_req);    
    return res.json({
        "Ok": true, 
        "UserGroup": {"Id":"id", "Name": "name", "Handle": "handle"}
    });
});

app.post("*", async (req, res) => {
    const raw_req = req.body;
    
    console.log(raw_req);    
    return res.json({success: true});
});


app.get("/api/usergroups.list", (req, res) => {
    const raw_req = req.body;
    
    console.log(raw_req);    
    return res.json({
        "Ok": true
    });
});

app.listen(port, () => {
    console.log("Slack API spy is listening on port " + port);
});
