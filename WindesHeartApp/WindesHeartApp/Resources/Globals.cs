using System.Collections.Generic;
using WindesHeartApp.ViewModels;
using WindesHeartSDK;
using Xamarin.Forms;

namespace WindesHeartApp.Resources
{
    public static class Globals
    {
        public static double screenHeight { get; set; }
        public static double screenWidth { get; set; }
        public static Color primaryColor { get; set; } = Color.FromHex("#96d1ff");
        public static Color secondaryColor { get; set; } = Color.FromHex("#53b1ff");
        public static Color headerColor { get; set; } = Color.FromHex("#234A97");
        public static Color lighttextColor { get; set; } = Color.FromHex("#999999");
        public static double buttonSize { get; set; }
        public static double screenratioFactor { get; set; }
        public static double buttonfontSize { get; set; }
        public static double cornerRadius { get; set; }

        public static Dictionary<string, Color> colorDictionary;
        public static int batteryPercentage { get; set; }
        public static int heartRate { get; set; }
        public static BLEDevice device;
        public static HeartrateViewModel hrviewModel;
        public static HomePageViewModel homepageviewModel;
        public static string batteryImage { get; set; }


        //buttonSize : 10 being biggest, 100 being smallest. 
        //buttonfontSize : 2-10, 10 being smallest, 2 being largest.
        public static void BuildGlobals()
        {
            batteryImage = "";
            buttonSize = 20;
            batteryImage = "";
            buttonfontSize = 4;
            cornerRadius = ((screenHeight / 10 * 1) - buttonSize);
            screenratioFactor = screenHeight / screenWidth;
            hrviewModel = new HeartrateViewModel();

            homepageviewModel = new HomePageViewModel();
            colorDictionary = new Dictionary<string, Color>
            {
                { "Aqua", Color.Aqua},
                { "Black", Color.Black},
                { "LightBlue (Default)", Color.FromHex("#96d1ff")}, { "Fucshia", Color.Fuchsia },
                { "Gray", Color.Gray }, { "Green", Color.Green },
                { "Lime", Color.Lime }, { "Maroon", Color.Maroon },
                { "Navy", Color.Navy }, { "Olive", Color.Olive },
                { "Purple", Color.Purple }, { "Red", Color.Red },
                { "Silver", Color.Silver }, { "Teal", Color.Teal },
                { "White", Color.White }, { "Yellow", Color.Yellow }
            };
        }
    };
}