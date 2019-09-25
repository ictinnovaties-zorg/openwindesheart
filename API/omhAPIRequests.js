var request = require('request');
module.exports ={
   sendToOpenmHealth: function sendToOpenmHealth(datapoint, user){
         var options = {
            url: 'http://85.214.42.74:8085/v1.0.M1/dataPoints',
            body: JSON.stringify(datapoint),
            headers: {  
                'Accept': 'application/json',
                'Authorization': `Bearer ${user.OauthAccessToken}`,
                'Content-Type': 'application/json'
            }
            };
            return new Promise(function(resolve, reject) {
                request.post(options, async function(err, resp, body) {
                    if (err) {
                        await reject(err);
                    } else {
                        await resolve(resp.statusCode);
                    }
                })
                })
    },
    getLastSync: function getLastSync(user){
           // Setting URL and headers for request
        var options = {
        url: 'http://85.214.42.74:8085/v1.0.M1/lastDataPoint?schema_namespace=omh&schema_name=heart-rate&schema_version=1.0',
        headers: {  
            'Accept': 'application/json',
            'Authorization': `Bearer ${user.OauthAccessToken}`
        }
        
        }; 
        
        // Return new promise 
        return new Promise(function(resolve, reject) {
            // Do async job
            request.get(options, function(err, resp, body) {
                if (err) {
                    reject(err);
                } else {
                    try{
                        const responseJSON = JSON.parse(body);
                        console.log(responseJSON);
                        resolve(responseJSON["body"]["effective_time_frame"]["date_time"]);
                    }
                    catch{
                        resolve();
                    }
                }
            })
            })

    }
}