module.exports ={
  ActivityMeasurement: function ActivityMeasurement(Timestamp, DeviceId, UserId, RawIntensity, Steps, RawKind, HeartRate) {
    this.Timestamp = Timestamp;
    this.DeviceId = DeviceId;
    this.UserId = UserId;
    this.RawIntensity = RawIntensity;
    this.Steps = Steps;
    this.RawKind = RawKind;
    this.HeartRate = HeartRate;
  },
  OmhUser: function OmhUser (OauthAccessToken, user_id){
    this.OauthAccessToken = OauthAccessToken;
  },
  UserMeasurement: function UserMeasurement(OmhUser, ActivityMeasurement){
    this.OmhUser = OmhUser;
    this.ActivityMeasurement = ActivityMeasurement;
  },
  heart_rate: function heart_rate(value){
    this.unit = "beats/min";
    this.value = value;
  },
  effective_time_frame: function effective_time_frame(date_time){
    this.date_time = date_time;
  },
  body: function body(effective_time_frame, heart_rate){
    this.effective_time_frame = effective_time_frame;
    this.heart_rate = heart_rate;
  },
  schema_id: function schema_id(){
    this.namespace = "omh",
    this.name = "heart-rate"
    this.version = "1.0"
  },
  acquisition_provenance: function acquisition_provenance(source_creation_date_time){
    this.source_name = "app";
    this.source_creation_date_time = source_creation_date_time;
    this.modality = "sensed";
  },
  header: function header(id, creation_date_time, acquisition_provenance, user_id, schema_id){
    this.id = id;
    this.creation_date_time = creation_date_time;
    this.acquisition_provenance = acquisition_provenance;
    this.user_id = user_id;
    this.schema_id = schema_id;    
  },
  Datapoint: function Datapoint(header, body, id){
    this.header = header;
    this.body = body;
    this.id = id;
  }
};