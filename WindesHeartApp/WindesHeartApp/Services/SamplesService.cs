using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;
using WindesHeartApp.Resources;
using WindesHeartSdk.Model;
using WindesHeartSDK;
using Xamarin.Forms;

namespace WindesHeartApp.Services
{
    public class SamplesService
    {
        private readonly IHeartrateRepository _heartrateRepository;
        private readonly IStepsRepository _stepsRepository;
        private readonly ISleepRepository _sleepRepository;

        private List<Step> _steps = new List<Step>();
        private List<Heartrate> _heartrates = new List<Heartrate>();
        private List<Sleep> _sleep = new List<Sleep>();

        public SamplesService(IHeartrateRepository heartrateRepository, IStepsRepository stepsRepository, ISleepRepository sleepRepository)
        {
            _heartrateRepository = heartrateRepository;
            _stepsRepository = stepsRepository;
            _sleepRepository = sleepRepository;
        }

        public async void StartFetching()
        {
            Device.BeginInvokeOnMainThread(delegate
            {
                Globals.HomePageViewModel.IsLoading = true;
                Globals.HomePageViewModel.ToggleEnableButtons();
            });
            var startDate = await GetLastAddedDateTime();
            Windesheart.ConnectedDevice.FetchData(startDate.AddMinutes(1), FillDatabase);

        }

        private async void FillDatabase(List<ActivitySample> samples)
        {

            Debug.WriteLine("Filling DB with samples");


            foreach (var sample in samples)
            {
                var datetime = sample.Timestamp;

                AddHeartrate(datetime, sample);
                AddStep(datetime, sample);
                AddSleep(datetime, sample);
            }

            await _heartrateRepository.AddRangeAsync(_heartrates);
            await _sleepRepository.AddRangeAsync(_sleep);
            await _stepsRepository.AddRangeAsync(_steps);

            _heartrateRepository.SaveChangesAsync();
            _stepsRepository.SaveChangesAsync();
            _sleepRepository.SaveChangesAsync();
            Debug.WriteLine("Fetched all samples");
            Device.BeginInvokeOnMainThread(delegate
            {
                Globals.HomePageViewModel.IsLoading = false;
                Globals.HomePageViewModel.ToggleEnableButtons();
            });
        }

        private async Task<DateTime> GetLastAddedDateTime()
        {
            var steps = await _stepsRepository.GetAllAsync();
            Debug.WriteLine("Steps db contains: " + steps.Count() + " entries");
            if (steps.Count() > 0)
            {
                Debug.WriteLine("Last added datetime is: " + steps.Last().DateTime);
                return steps.Last().DateTime;
            }
            Debug.WriteLine("Last added datetime is: " + DateTime.Now.AddDays(-30));
            return DateTime.Now.AddDays(-30);
        }

        private void AddHeartrate(DateTime datetime, ActivitySample sample)
        {
            var heartRate = new Heartrate(datetime, sample.HeartRate != 255 ? sample.HeartRate : 0);
            _heartrates.Add(heartRate);
            // await _heartrateRepository.AddAsync(heartRate);
        }

        private void AddStep(DateTime datetime, ActivitySample sample)
        {
            var step = new Step(datetime, sample.Steps);
            _steps.Add(step);
            // await _stepsRepository.AddAsync(step);
        }

        private void AddSleep(DateTime datetime, ActivitySample sample)
        {
            Sleep sleep;
            switch (sample.Category)
            {
                case 112:
                    sleep = new Sleep(datetime, SleepType.Light);
                    break;
                case 121:
                case 122:
                case 123:
                    sleep = new Sleep(datetime, SleepType.Deep);
                    break;

                default:
                    sleep = new Sleep(datetime, SleepType.Awake);
                    break;
            }
            _sleep.Add(sleep);
            // await _sleepRepository.AddAsync(sleep);
        }

        public void EmptyDatabase()
        {
            _heartrateRepository.RemoveAll();
            _stepsRepository.RemoveAll();
            _sleepRepository.RemoveAll();
        }
    }
}
