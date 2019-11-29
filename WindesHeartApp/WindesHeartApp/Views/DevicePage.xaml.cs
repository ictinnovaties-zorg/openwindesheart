using FormsControls.Base;
using WindesHeartApp.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [System.Runtime.InteropServices.Guid("A63B9823-FB43-4942-BAAA-5F02EAF86AC8")]
    public partial class DevicePage : ContentPage, IAnimationPage
    {
        public static ListView devicelist;
        public DevicePage()
        {
            InitializeComponent();
            BuildPage();
        }

        private void BuildPage()
        {
            absoluteLayout = new AbsoluteLayout();
            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Device", 0.05, 0.10, Globals.LightTextColor, "", 0);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            #region scanbutton
            Button scanButton = PageBuilder.AddButton(absoluteLayout, "Scan for devices", "ScanButtonCommand", 0.15, 0.25, 0.3, 0.08, AbsoluteLayoutFlags.All);
            scanButton.CornerRadius = (int)Globals.ScreenHeight / 100 * 7;
            scanButton.FontSize = Globals.ScreenHeight / 100 * 2;
            #endregion
            PageBuilder.AddLabel(absoluteLayout, "", 0.80, 0.25, Globals.LightTextColor, "StatusText", 14);

            var indicator = PageBuilder.AddActivityIndicator(absoluteLayout, "IsLoading", 0.80, 0.28, 50, 50, AbsoluteLayoutFlags.PositionProportional, Globals.SecondaryColor);


            DataTemplate DeviceTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition {Width = Globals.ScreenWidth / 100 * 25},
                        new ColumnDefinition {Width = Globals.ScreenWidth / 100 * 25},
                        new ColumnDefinition {Width = Globals.ScreenWidth / 100 * 25},
                        new ColumnDefinition {Width = Globals.ScreenWidth / 100 * 25}
                    }
                };

                Label label = new Label();
                label.SetBinding(Label.TextProperty, "Name");
                grid.Children.Add(label, 0, 0);


                Label label2 = new Label { HorizontalOptions = LayoutOptions.End };
                label2.SetBinding(Label.TextProperty, "Rssi");
                grid.Children.Add(label2, 2, 0);

                Label label3 = new Label { Text = "Signal strength:", HorizontalOptions = LayoutOptions.Start };
                grid.Children.Add(label3, 1, 0);

                return new ViewCell { View = grid };
            });

            #region device ListView

            devicelist = new ListView { BackgroundColor = Globals.SecondaryColor, ItemTemplate = DeviceTemplate };
            devicelist.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedDevice", BindingMode.TwoWay));
            devicelist.SetBinding(ListView.ItemsSourceProperty, new Binding("DeviceList"));
            AbsoluteLayout.SetLayoutBounds(devicelist, new Rectangle(0.5, 0.55, 0.90, 0.4));
            AbsoluteLayout.SetLayoutFlags(devicelist, AbsoluteLayoutFlags.All);
            absoluteLayout.Children.Add(devicelist);
            #endregion

            #region disconnectButton
            Button disconnectButton = PageBuilder.AddButton(absoluteLayout, "Disconnect", "DisconnectButtonCommand", 0.15, 0.85, 0.3, 0.05, AbsoluteLayoutFlags.All);
            disconnectButton.CornerRadius = (int)Globals.ScreenHeight / 100 * 7;
            disconnectButton.FontSize = Globals.ScreenHeight / 100 * 2;
            #endregion

        }
        public IPageAnimation PageAnimation { get; } = new SlidePageAnimation { Duration = AnimationDuration.Short, Subtype = AnimationSubtype.FromTop };

        public void OnAnimationStarted(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }

        public void OnAnimationFinished(bool isPopAnimation)
        {
            // Put your code here but leaving empty works just fine
        }
    }
}

