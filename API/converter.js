const model = require('./model');
const uuidv4 = require('uuid/v4');
module.exports ={
    requestToMeasurement: function requestToMeasurement(requestmeasurement){
        ActivityMeasurement =  new model.ActivityMeasurement(
                                                            requestmeasurement["timestamp"],
                                                            requestmeasurement["deviceId"],
                                                            requestmeasurement["userId"],
                                                            requestmeasurement["rawIntensity"],
                                                            requestmeasurement["steps"],
                                                            requestmeasurement["rawKind"],
                                                            requestmeasurement["heartRate"]
                                                            );

        return ActivityMeasurement;
    },
    requestToOmhUser: function requestToOmhUser(requestuser){
        user = new model.OmhUser(
            requestuser["token"]
        );

        return user;
    },
    measurementToDatapoint: function measurementToDatapoint(user, measurement){
        const id = uuidv4();
        const schema_id = new model.schema_id();
        const acquisition_provenance = new model.acquisition_provenance(measurement.Timestamp);                                                 
        const header = new model.header(id, measurement.Timestamp, acquisition_provenance, user.user_id, schema_id);
        const effective_time_frame = new model.effective_time_frame(measurement.Timestamp);
        const heartRate = new model.heart_rate(measurement.HeartRate);
        const body = new model.body(effective_time_frame, heartRate);
        const datapoint = new model.Datapoint(header,body,id);

        return datapoint;
    }
}