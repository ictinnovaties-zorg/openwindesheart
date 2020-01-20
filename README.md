
# OpenWindesheart

The open-source OpenWindesheart is a library used for scanning, connecting and getting data from activity-trackers using bluetooth.

## Support

This project has been created by students of Windesheim University of Applied Sciences and does not guarantee any support in the future. 

This library uses the [ACR Reactive BluetoothLE Plugin](https://github.com/aritchie/bluetoothle) to do anything BluetoothLE related. Our project only supports versions that are supported by this library, because our project depends heavily upon it.

The features have been tested with Android/iOS phones in combination with the Mi Band 3/Xiaomi Band 3.  

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

## Supported Devices

At this moment the OpenWindesheart library only supports the Mi Band 3. 

The library is designed in a way that other devices can be added easily. If you want to add support for a device, please check out our documentation: [How to add support for a new device](https://github.com/ictinnovaties-zorg/openwindesheart/blob/master/README-SupportNewDevice.md)

## Documentation

#### User docs:
* [Scanning](https://github.com/ictinnovaties-zorg/openwindesheart/blob/master/README-Scanning.md)  
* [Connecting & Disconnecting](https://github.com/ictinnovaties-zorg/openwindesheart/blob/master/README-Connecting.md)  
* [Reading data](https://github.com/ictinnovaties-zorg/openwindesheart/blob/master/README-Readingdata.md)
* [Reading samples](https://github.com/ictinnovaties-zorg/openwindesheart/blob/master/README-Samples.md)
* [Supported settings](https://github.com/ictinnovaties-zorg/openwindesheart/blob/master/README-Settings.md)

#### Contributor docs:
* [How to add support for a new device](https://github.com/ictinnovaties-zorg/openwindesheart/blob/master/README-SupportNewDevice.md)

## SETUP  
1.  Clone this repository into your solution and manage a dependency from your mobile-project to this one.  
2.  Carefully read the docs for implementation of different features. For a working example, have a look at the Xamarin Forms project on this page.
3.  Implement the features in your mobile-application the way you want them!

## Credits

We would like to thank Allan Ritchie for creating the [ACR Reactive BluetoothLE Plugin](https://github.com/aritchie/bluetoothle). Without this open-source library our project would not have been finished within the specified amount of time.

## Contributions

To make contributions to this project, please open up a [pull request](https://github.com/ictinnovaties-zorg/openwindesheart/pull/new/master).

## Creators

* R. Abächerli [@ramonb1996](https://github.com/ramonB1996)
* H. van der Gugten [@hielkeg](https://github.com/hielkeg)
* T.C. Marschalk [@marstc](https://github.com/marstc)
* K. van Sloten [@kevinvansloten](https://github.com/kevinvansloten)

## Copyright

Copyright 2020 Research group ICT innovations in Health Care, Windesheim University of Applied Sciences.
