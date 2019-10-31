using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WindesHeartApp.Resources
{
    public static class Globals
    {
        public static double screenHeight { get; set; }
        public static double screenWidth { get; set; }
        public static Color primaryColor = Color.FromHex("#96d1ff");
        public static double buttonSize = ((screenHeight / 8) / screenHeight);
        public static double screenratioFactor = screenHeight / screenWidth;

    };
}