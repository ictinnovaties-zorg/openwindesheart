using Microcharts;
using SkiaSharp;
using System.Collections.Generic;
using WindesHeartApp.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace WindesHeartApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StepsPage : ContentPage
    {
        public static Label CurrentStepsLabel;
        public static Button ToggleRealTimeStepsButton;

        List<Entry> Entries = new List<Entry>();

        public StepsPage()
        {
            InitializeComponent();

            FillChart();
            chartView.Chart = new BarChart { Entries = Entries };
        }

        protected override void OnAppearing()
        {
            BindingContext = Globals.StepsViewModel;
            BuildPage();
        }

        public void FillChart()
        {
            AddStep("Monday", 73);
            AddStep("Tuesday", 89);
            AddStep("Wednesday", 120);
            AddStep("Thursday", 50);
            AddStep("Friday", 33);
            AddStep("Saturday", 620);
            AddStep("Sunday", 740);
        }

        public void AddStep(string labelText, int value, string hexColor = "#266489")
        {
            Entries.Add(
                new Entry(value)
                {
                    Color = SKColor.Parse(hexColor),
                    Label = labelText,
                    ValueLabel = value.ToString()
                });
        }

        private void BuildPage()
        {
            AbsoluteLayout absoluteLayout = new AbsoluteLayout();

            PageBuilder.BuildPageBasics(absoluteLayout, this);
            PageBuilder.AddHeaderImages(absoluteLayout);
            PageBuilder.AddLabel(absoluteLayout, "Steps", 0.10, 0.10);
            PageBuilder.AddReturnButton(absoluteLayout, this);

            CurrentStepsLabel = PageBuilder.AddLabel(absoluteLayout, "Steps:", 0.4, 0.2);
            CurrentStepsLabel.TextColor = Color.Black;
            CurrentStepsLabel.SetBinding(Label.TextProperty, new Binding("StepsLabelText"));

            PageBuilder.AddButton(absoluteLayout, "Get steps", "GetStepsBinding", 0.2, 0.28, 300, 50);
            ToggleRealTimeStepsButton = PageBuilder.AddButton(absoluteLayout, "Enable realtime steps", "ToggleRealTimeStepsBinding", 0.2, 0.38, 300, 50);

            AbsoluteLayout.SetLayoutFlags(chartView, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(chartView, new Rectangle(0.2, 0.75, 300, 250));
            absoluteLayout.Children.Add(chartView);
        }
    }
}