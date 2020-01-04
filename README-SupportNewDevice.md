
# How to add support for a new device

This documentation offers a step by step guide on how to add support for a new device. We assume you have the WindesheartSDK project cloned and open in Visual Studio.

### 1. Add new device folder
![alt text](https://i.imgur.com/NBTx5ao.png)
To start, you need a folder to put all your code in. In the Devices folder, create a new folder named after your device. Please follow the format as shown  in the MiBand3 folder. Put your models inside a Models folder, Helpers in a Helpers folder, and so on.

### 2. Add your own Device class extending BLEDevice
Inside your newly created folder, add a "Models" folder, then create a new class in there. 
This class should have the exact same name as your folder:
![alt text](https://i.imgur.com/TsCBYPR.png)


The class also has to extend from BLEDevice. Because of this, you're forced to implement all abstract methods that class contains.

![Alt text](https://i.imgur.com/EfJguV1.png)
In theory this class can do all the logic for your device, however it is **STRONGLY** recommended that you have different services that handle the implementation, as seen in MiBand3.cs:

![Alt text](https://i.imgur.com/AVkwYgb.png)

A few things you need to have in this class:
* Set BLEDevice's ConnectionCallback of BleDevice in your Connect method.
* Set BLEDevice's DisconnectionCallback of BleDevice in your SubscribeToDisconnect method.

As seen in the example below:

![Alt text](https://i.imgur.com/YBlvsiq.png)



## 3. Add your code
Now its time to implement everything! To keep things organized and clean, please use different services for different tasks. Mi Band 3 is a good example for this.

If your device doesn't support an action provided by the BLEDevice class, just throw a NotImplementedException. 
For example, if your device doesn't support sleep tracking, you do this:
![Alt text](https://i.imgur.com/PiEXzim.png)

## 4. Edit the GetDevice method
The last step is to edit the GetDevice method inside of  `BluetoothService.cs`
This method returns the correct BLEDevice dependent of it's name. Please note that `var name` is the Bluetooth device name. There might be two or more names for one device (in case of the Mi Band 3).
![Alt text](https://i.imgur.com/wdEOTcZ.png)
And that should be it! Now, when scanning for devices it should detect and return your device! Please make sure everything works as expected before submitting a pull request.

[<---- Back to mainpage](https://bitbucket.org/ictinnovaties-zorg/openwindesheart/src/master/)