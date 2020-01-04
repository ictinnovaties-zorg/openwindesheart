
# Scanning

Scanning allows the user to scan for BLEDevices.

### Windesheart.StartScanning(Callback)
This method starts the scanning progress and calls the provided callback method when a BLEDevice is found. It returns a boolean to indicate whether scanning has started. An example:



```csharp
public void Main(){
   if(Windesheart.StartScanning(WhenDeviceFound)){
	Console.WriteLine("Scanning started!");
   }
   else
   {
	Console.WriteLine("Couldn't start scanning. Is Bluetooth turned on?");
   }
}

public void WhenDeviceFound(BLEScanResult result){
   Console.WriteLine("Device found!"); 
   BLEDevice device = result.device;
   int Rssi = result.Rssi;
   AdvertisementData data = result.AdvertismentData;
}
```
As you can see in `WhenDeviceFound`, a BLEScanResult parameter is given. This parameter contains the device itself, the initial Rssi of the device, and the AdvertisementData for that device.
 
When using this method, be sure to call the `Windesheart.StopScanning()` method at some point as well, otherwise your device will continue scanning indefinitely.

### Windesheart.StopScanning()
This method stops the scanning process. This is a void method. You can assume that this method always succeeds.
```csharp
Windesheart.StartScanning(WhenDeviceFound); //Start scanning
await Task.Delay(2000); //Wait 2 seconds
Windesheart.StopScanning(); //Stop scanning
```

[<---- Back to mainpage](https://bitbucket.org/ictinnovaties-zorg/openwindesheart/src/master/)