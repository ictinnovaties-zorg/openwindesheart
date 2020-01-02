# Reading Samples
The Windesheart SDK allows reading samples from the past from BLE Devices.

## *Fetching Samples*

By using our *MiBand3SamplesService* it is possible to fetch samples from the past days, weeks, months or even years.

Samples are generated once *every minute* in your device. This means that fetching data for 1 day, 1440 samples will be fetched (24 hours * 60 minutes = 1440 samples).

The data that is generated will be stored in a class called *ActivitySample*. This class contains the following data:

* **RawData**: This is the byte-array containing the raw-data of the sample (for debugging purposes).
* **TimeStamp**: The exact DateTime that this sample contains data for.
* **UnixEpochTimeStamp**: The amount of seconds since UnixEpoch. This way time-zones can be countered.
* **Category**: This is a number that tells you the sort of activity that has been measured in that minute. Walking, running, sleeping and other activities all have their separate number. We are unsure in what all the categories are, but some are obvious in our opinion. **NOTE: The accuracy of this number can vary. Sometimes the Mi band 3 thinks you are sleeping, when in reality you are sitting without moving.**
* **RawIntensity**: This number can tell you whether your activity was intense or not. If you walk faster, the measured intensity will most likely be higher.
* **Steps**: This is the number of steps that have been measured in that minute. If you use addition on all the step-numbers of the current day, then this should be exactly or close to your displayed step-number on your device.
* **HeartRate**: The measured heartrate in that minute. **NOTE: This only gets measured if you wear your device properly and have set the heartratemeasurement-interval to 1 minute by using *MiBand3.SetHeartrateMeasurementInterval(1)***

A list of AcitivitySample can be acquired by calling *MiBand3SampleService.StartFetching(). You will have to provide the datetime of the starting date (Example: For a week of data you will call this method with DateTime.Now.AddDays(-7)).

The callbacks are useful if you want to know when fetching has been finished and if you want to get a callback on every 250 samples for creating a progressbar in the mobile-app.

Fetching can take a while, because of the speed of Bluetooth Low Energy. Storing data in a database and only fetching the data of the last synchronization will speed up the process alot.

## *Example*

To get the list of samples of your device correctly, you will have to connect to the device first and then use this example:

```csharp
//Fetching from a week ago until now
DateTime startDate = DateTime.Now.AddDays(-7);
Windesheart.PairedDevice.GetSamples(startDate, FetchingFinished, UpdateProgression);

private void UpdateProgression(int remainingSamples) {
    /*Do something with the calculated remaining samples.
    For example: remainingSamples = 1440,
    means that this amount of samples is remaining
    until all samples have been fetched.*/
}

private void FetchingFinished(List<ActivitySample> samples) {
    /*Do something with the samples that have been found*/
}
```

*The demo-application contains a working example of this in the directory 'Services' -> 'SamplesService.cs'*

[<---- Back to mainpage](https://bitbucket.org/ictinnovaties-zorg/openwindesheart/src/master/)