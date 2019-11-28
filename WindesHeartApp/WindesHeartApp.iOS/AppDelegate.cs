
using Foundation;
using System;
using System.IO;
using UIKit;
using WindesHeartApp.Data.Repository;
using WindesHeartApp.Resources;

namespace WindesHeartApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            SQLitePCL.Batteries.Init();

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library",
                "WindesheartApp.db");
            Globals.HeartrateRepository = new HeartrateRepository(dbPath);
            Globals.StepsRepository = new StepsRepository(dbPath);

            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            Globals.screenHeight = (int)UIScreen.MainScreen.Bounds.Height;
            Globals.screenWidth = (int)UIScreen.MainScreen.Bounds.Width;
            return base.FinishedLaunching(app, options);
        }
    }
}
