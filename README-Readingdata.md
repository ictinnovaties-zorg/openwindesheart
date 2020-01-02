# Reading data
The Windesheart SDK allows reading data from BLE Devices. 


## Reading current data
The data that can be read at the moment is:

* Battery data by using  `BLEDevice.GetBattery()`
* Steps by using  `BLEDevice.GetSteps()`

Please note that all of this is **current** data. If you want data of the past, you have to get the samples of your BLE Device.

**Example**

```csharp
BLEDevice device = // your device
StepData steps = device.GetSteps();
int stepCount = steps.StepCount;

BatteryData battery= device.GetBattery();
BatteryStatus status = battery.Status; //Either Charging or NotCharging
int percentage = battery.Percentage;
```


## Real time data
There is also an option to continuously get data from your device. This works by providing a callback method that will be run when the data has been updated.

This works with:

 - Steps by using  `BLEDevice.EnableRealTimeSteps(StepData)`
 - Battery by using  `BLEDevice.EnableRealTimeBattery(BatteryData)`
 - Heart rate by using  `BLEDevice.EnableRealTimeHeartrate(HeartrateData)`

**Example**
Let's say we want to continuously get the steps of the device for a period of 1 minute, then we could do this:

```csharp

async void Main(){
   device.EnableRealTimeSteps(onStepsUpdated);
   await Task.Delay(60000);
   device.DisableRealTimeSteps(); //Don't forget to disable when not needed anymore!
}

void OnStepsUpdated(StepData data){
   int steps = data.StepCount;
   Console.WriteLine("Steps updated: " + steps);
}
```

