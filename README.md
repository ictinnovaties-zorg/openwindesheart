# WindesHeart SDK for Mi Band 3

This project is an easy-to-use, cross-platform way of using a BluetoothLE device, like a Mi Band 3, in your own mobile application.  

## Support

This project has been created by students of Windesheim and does not guarantee any support in the future.  

This SDK uses the [ACR Reactive BluetoothLE Plugin](https://github.com/aritchie/bluetoothle) to do anything BluetoothLE related. Our project only supports versions that are supported by this library, because we depend heavily upon it.

The features have been tested with Android/iOS phones in combination with the Mi Band 3/Xiaomi Band 3.  

As this SDK has not been tested with other devices, we can not guarantee a working product for anything else than the Mi Band 3.

## Features

* Scanning for BLE devices
* Pairing with a BLE device
* Connecting to BLE device
* Reading current battery-data and status
* Reading current step-data
* Reading step-data from the past days
* Reading heartrate after measurement.
* Reading heartrate from the past days with measurement-intervals
* Reading sleep-data from the past days
* Setting step-goal on BLE device
* Setting language on BLE device
* Setting Date and time on BLE device
* Setting Date-format and Hour-notation on BLE device
* Toggle screen of BLE device on wrist-lift
* Auto-reconnect after Bluetooth-adapter toggle
* Auto-reconnect when within range of BLE device
* Disconnect with BLE device

## Documentation

* [Scanning](https://bitbucket.org/ictinnovaties-zorg/openwindesheart/src/master/README-Scanning.md)  
* [Connecting](https://bitbucket.org/ictinnovaties-zorg/openwindesheart/src/master/README-Connecting.md)  
* [Reading data](https://bitbucket.org/ictinnovaties-zorg/openwindesheart/src/master/README-Readingdata.md)
* [Reading samples](https://bitbucket.org/ictinnovaties-zorg/openwindesheart/src/master/README-Samples.md)
* [Supported settings](https://bitbucket.org/ictinnovaties-zorg/openwindesheart/src/master/README-Settings.md)
* [Disconnecting](https://bitbucket.org/ictinnovaties-zorg/openwindesheart/src/master/README-Disconnecting.md)

## SETUP  

* Clone this repository into your solution and manage a dependency from your mobile-project to this one.  

* Carefully read the docs for implementation of different features. For a working example, have a look at the Xamarin Forms project on this page.

* Implement the features in your mobile-application the way you want them!

## Credits

We would like to thank Hogeschool Windesheim and the lecturate for ICT Innovations in healthcare for giving us this assignment.  

Further we would like to thank Allan Ritchie for creating the [ACR Reactive BluetoothLE Plugin](https://github.com/aritchie/bluetoothle). Without this open-source library our project would not have been finished within the specified amount of time.
