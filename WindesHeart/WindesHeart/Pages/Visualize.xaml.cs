using System;
using System.Collections.Generic;
using SkiaSharp;
using Entry = Microcharts.Entry;
using System.Linq;
using WindesHeartSdk.MiBand;
using WindesHeartSdk.Model;
using Microcharts;

namespace WindesHeart.Pages
{
    public partial class Visualize
    {
        public Visualize()
        {
            InitializeComponent();
            create_listAsync();
        }

        public async void create_listAsync()
        {
            List<MiBandActivitySample> daySamples = Get24HourSamples(5);
    
            List<MiBandActivitySample> data = await MiBandDb.Database.GetTodaySamples();

            var dataDict = data.ToDictionary(x => x.Timestamp.ToLocalTime(), x => x.HeartRate);

            daySamples.Select(x =>
            {
                dataDict.TryGetValue(x.Timestamp, out var value);
                x.HeartRate = value < 255 ?  value : 0;
                return x;
            }).ToList();

            List<Entry> measurements = daySamples.Select(x => new Entry(x.HeartRate)
            {
                Color = SKColor.Parse("#FF1943"),
                Label = x.Timestamp.Minute == 0? x.Timestamp.ToString("t") : null
            }).ToList();
            Chart1.Chart = new LineChart()
            {
                Entries = measurements,
                LineMode = LineMode.Straight
            };
        }

        private List<MiBandActivitySample> Get24HourSamples(int interval)
        {
            List<MiBandActivitySample> daySamples = Enumerable
                .Range(0, (int)(new TimeSpan(24, 0, 0).TotalMinutes / interval))
                .Select(i => new MiBandActivitySample()
                {
                    Timestamp = DateTime.Today.AddMinutes(i * (double)interval)
                }).ToList();

            return daySamples;
        }
    }
}
