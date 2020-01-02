# Settings

Most devices have some settings that can be changed. The Mi Band 3 is not an exception and that is why we created a way of changing some basic settings for this device.

## How to use

The following settings can be configurated to your liking for the Mi band 3, by using *Windesheart.PairedDevice* when already connected to the device:

* **Display Language**: You can change your display language to any locale you wish, by calling the SetLanguage(string localeString) method with the preferred locale.
**NOTE: We have not tested all locales that exist, but german(de-DE), dutch(nl-NL) and english(en-EN) definitely work.
[A list of locales that could work can be found here.](https://stackoverflow.com/questions/3191664/list-of-all-locales-and-their-short-codes)

* **Set the time-displayformat**: You can change the displayformat for time to 12-hour or 24-hour format. Call the SetTimeDisplayFormat(bool is24hours) to change this.

* **Set the date-displayformat**: You can change the displayformat for dates to either dd-MM-YYYY or MM-dd-YYYY. Use the method SetDateDisplayFormat(bool isddMMYYYY) for this.

* **Set Stepgoal**: You can set a goal for the amount of steps needed per day. By using the SetStepGoal(int steps) you can set the goal to your liking. You should also call EnableFitnessGoalNotification(bool enable) to actually receive the notification on the device when the goal has been reached. **NOTE: If the goal will be set to 1000 steps (with the notification on) and you already have more than 1000 steps displayed on your device, then this will trigger the next time you take a step.**

* **Set Time**: You can change the time that will be displayed on your device. This can be done by calling SetTime(DateTime dateTime) with the datetime you want the time to be set to. **NOTE: Changing this could result in loss of samples and can unreliably change data that can be received from the device.**

* **Activate display on wristlift**: If you want to turn on your display of your device, when lifting your wrist, then use the method SetActivateOnLiftWrist(bool activate). You can also use SetActivateOnLiftWrist(DateTime from, DateTime to) to activate this feature between the two dates.

* **Enable sleep-tracking**: If you would like to create samples of your sleeping pattern, then this configuration has to be turned on! Use EnableSleepTracking(bool enable) to toggle the functionality.

* **Set Heartrate-measurementinterval**: If you would like more accurate samples with heartrates, then you should turn on this function for the interval you want. Use SetHeartrateMeasurementInterval(int minutes) to set this interval. **NOTE: It is recommended to use SetHeartrateMeasurementInterval(1) to measure your heartrate automatically every minute. More samples will have accurate heartrate-data this way if the device is worn correctly. This does consume more battery, so be aware of this!**

*It is recommended to set these features once when connecting to the device. This way you will not forget them.*


[<---- Back to mainpage](https://bitbucket.org/ictinnovaties-zorg/openwindesheart/src/master/)