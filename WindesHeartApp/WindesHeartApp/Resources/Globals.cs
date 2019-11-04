using Xamarin.Forms;

namespace WindesHeartApp.Resources
{
    public static class Globals
    {
        public static double screenHeight { get; set; }
        public static double screenWidth { get; set; }
        public static Color primaryColor { get; set; }
        public static double buttonSize { get; set; }
        public static double screenratioFactor { get; set; }
        public static double buttonfontSize { get; set; }
        public static double cornerRadius { get; set; }


        //buttonSize : 10 being biggest, 100 being smallest. 
        //buttonfontSize : 2-10, 10 being smallest, 2 being largest.

        public static void BuildGlobals()
        {
            buttonSize = 20;
            buttonfontSize = 4;
            primaryColor = Color.FromHex("#96d1ff");
            cornerRadius = ((screenHeight / 10 * 1) - buttonSize);
            screenratioFactor = screenHeight / screenWidth;
        }
    };
}