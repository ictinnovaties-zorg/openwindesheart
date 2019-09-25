const env = require('dotenv').config({path: __dirname + '/.env'});
const express = require('express');
const app = express();
const model = require('./model');
const converter = require('./converter');
const omhAPIRequest = require('./omhAPIRequests');
app.use(express.json());


app.post('/pushdatapoints',(req, res) =>{
    const request = req.body;
    let i = 0;
    function syncLoop(request, i){
        if (i <  request["measurements"].length){
            const measurement = converter.requestToMeasurement(request["measurements"][i]);
            const user = converter.requestToOmhUser(request["user"]);
            const datapoint = converter.measurementToDatapoint(user, measurement);
            let initializePromise = omhAPIRequest.sendToOpenmHealth(datapoint, user);
            initializePromise.then(function(result){
                if (result == 201){
                    console.log(i)
                    if (i < request["measurements"].length -1){
                        i++;
                        syncLoop(request, i);
                    }
                    else {
                        res.status(result);
                        res.send("success");
                    }   
                }
                else{
                    res.status(result);
                    res.send("failure");
                }
            });
        }
        else if (!request["measurements"].length){
            res.status(200)
            res.send("already up to date")
        }
    };
    syncLoop(request, i);
});



app.post('/getlastsync', (req, res) =>{
        const request = req.body;
        const user = converter.requestToOmhUser(request);
        let initializePromise = omhAPIRequest.getLastSync(user);
        var lastSync;
        initializePromise.then(function(result){
            lastSync = result;
            res.send(lastSync)
        });
    });

const port = process.env.PORT || 3000;
app.listen(port, ()=> console.log(`listening on port ${port}`));