using FormsControls.Base;
using WindesHeartApp.Resources;
using WindesHeartApp.Services;
using WindesHeartSDK;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevicePage : IAnimationPage
    {
        public static ListView Devicelist;
        public static Button ScanButton;
        public static Button DisconnectButton;
        public static ImageButton ReturnButton;

        public DevicePage()
        {
            InitializeComponent();
            BuildPage();
        }

        private void BuildPage()
        {
            #region absoluteLayout 
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Device", 0.05, 0.10, Globals.LightTextColor, "", 0);
            ReturnButton = PageBuilder.AddReturnButton(absoluteLayout);
            
            ScanButton = PageBuilder.AddButton(absoluteLayout, "", Globals.DevicePageViewModel.ScanButtonClicked, 0.15, 0.25, 120, 50, 14, 12, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            ScanButton.SetBinding(Button.TextProperty, "ScanButtonText");
            PageBuilder.AddActivityIndicator(absoluteLayout, "IsLoading", 0.50, 0.25, 50, 50, AbsoluteLayoutFlags.PositionProportional, Globals.LightTextColor);
            PageBuilder.AddActivityIndicator(absoluteLayout, "IsLoading", 0.50, 0.25, 50, 50, AbsoluteLayoutFlags.PositionProportional, Globals.LightTextColor);
            PageBuilder.AddLabel(absoluteLayout, "", 0.80, 0.25, Globals.LightTextColor, "StatusText", 14);
            #endregion

            #region device ListView
            var deviceTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {Width = Globals.ScreenWidth / 100 * 33},
                        new ColumnDefinition {Width = Globals.ScreenWidth / 100 * 33},

                        new ColumnDefinition {Width = Globals.ScreenWidth / 100 * 33},
                    }
                };

                Label label = new Label { FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 12 };
                label.SetBinding(Label.TextProperty, "Device.Name");
                grid.Children.Add(label, 0, 0);


                Label label2 = new Label { VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Start, FontAttributes = FontAttributes.Italic, FontSize = 12 };
                label2.SetBinding(Label.TextProperty, "Rssi");

                grid.Children.Add(label2, 2, 0);

                Label label3 = new Label { Text = "Signal strength:", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = 12 };
                grid.Children.Add(label3, 1, 0);

                return new ViewCell { View = grid };
            });
            Devicelist = new ListView { BackgroundColor = Globals.SecondaryColor, ItemTemplate = deviceTemplate };
            Devicelist.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedDevice", BindingMode.TwoWay));
            Devicelist.SetBinding(ListView.ItemsSourceProperty, new Binding("DeviceList"));
            AbsoluteLayout.SetLayoutBounds(Devicelist, new Rectangle(0.5, 0.55, 0.90, 0.4));
            AbsoluteLayout.SetLayoutFlags(Devicelist, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(Devicelist);
            #endregion

            #region disconnectButton
            DisconnectButton = PageBuilder.AddButton(absoluteLayout, "Disconnect", Globals.DevicePageViewModel.DisconnectButtonClicked, 0.15, 0.85, 120, 50, 14, 12, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);
            #endregion
        }

        protected override void OnDisappearing()
        {
            Globals.DevicePageViewModel.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            if (Windesheart.PairedDevice != null)
            {
                if (Windesheart.PairedDevice.IsConnected())
                {
                    DisconnectButton.IsEnabled = true;
                }
            }
            DisconnectButton.IsEnabled = false;
        }

        #region pageAnimation
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromTop };

        public void OnAnimationStarted(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }
        #endregion
    }
}

