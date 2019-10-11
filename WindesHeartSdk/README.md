# WindesHeart SDK

This package add functionalities to extract data from a Mi Band 3 and send it over to an Open mHealt server.

## Features

- Pair to a Mi Band 3
- Automaticly reconnect after disconnect
- Fetch activity data
- Get connection and battery status updates
- Send activity data to an Open mHealt server

## Usage

### BleService 
#### Setup
Initialize the BleService in the App.xaml.cs so it can be used from everywhere in the app
```
public static BleService BleService { get; set; }

public App()
	{
		InitializeComponent();
		BleService = new BleService();
	}
```

#### Listen for connection status changes
```
BleService.ConnectionStatusSubject.Subscribe(status => 	{
		//Do something
	}		
```

#### Listen for pairing status changes
```
BleService.PairResultSubject.Subscribe(result => {
		//Do something
	}		
```

#### Scan and connect to nearest Mi Band 3 
```
BleService.ScanNearbyDevices();
```

#### Connect to known Mi Band 3
```
BleService.ConnectKnownDevice();
```

#### Disconnect Mi Band 3
```
BleService.DisconnectDevice();
```

### MiBandSupport
#### Bind function to battery info changes
```
await MiBandSupport.OnBatteryStatusChange(BatteryChange);

void BatteryChange(BatteryInfo info) {
		// Do something
	}
```

#### Set Mi Band 3 settings

```
await MiBandSupport.SetCurrentTime()

await MiBandSupport.SetActivateDisplayOnLiftWrist(bool)

await MiBandSupport.SetActivateDisplayOnLiftWrist(int)
```

#### Restore settings to default
Sets SetActivateDisplayOnLiftWrist to false and SetActivateDisplayOnLiftWrist to 1

```
await MiBandSupport.RestoreSettings()
```

### FetchOperation

#### Fetch activity data
```
FetchOperation fetchOperation = new FetchOperation();

fetchOperation.InitiateFetching().Subscribe(async isFetching => {
		//Do something (e.g. notify activity data being fetched)
	}
```

#### Send saved datapoints to Open mHealt server
Set the endpoints for the Open mHealt server
```
RestService.ApiUri = "{your api url}";
RestService.DsuUri = "{your dsu url}/oauth/token";
```

Send localy stored datapoints to server
```
await RestService.PushDataPoints()
```

### MiBandActivityDatabase

#### Get last saved activity sample
```
await MiBandActivityDatabase.GetLastSample()
```

#### Delete all activity samples from local db
```
await MiBandActivityDatabase.DeleteAllAsync()
```

## Credits

Huge thanks to [Gadgetbridge](https://github.com/Freeyourgadget/Gadgetbridge) that made our project possible

