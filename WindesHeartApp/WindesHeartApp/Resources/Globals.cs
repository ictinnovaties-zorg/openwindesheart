using Xamarin.Forms;

namespace WindesHeartApp.Resources
{
    public static class Globals
    {
        public static double screenHeight { get; set; }
        public static double screenWidth { get; set; }
        public static Color primaryColor = Color.FromHex("#96d1ff");
        //buttonsize: 10 being biggest, 100 being smallest. 
        public static double buttonSize = 20;
        public static double screenratioFactor = screenHeight / screenWidth;

    };
}