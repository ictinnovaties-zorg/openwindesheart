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

namespace WindesHeartApp.Services
{
    public class SamplesService
    {
        private readonly IHeartrateRepository _heartrateRepository;
        private readonly IStepsRepository _stepsRepository;
        private readonly ISleepRepository _sleepRepository;

        public SamplesService(IHeartrateRepository heartrateRepository, IStepsRepository stepsRepository, ISleepRepository sleepRepository)
        {
            this._heartrateRepository = heartrateRepository;
            this._stepsRepository = stepsRepository;
            this._sleepRepository = sleepRepository;
        }

        public async void StartFetching()
        {
            Globals.HomePageViewModel.IsLoading = true;
            var startDate = await GetLastAddedDateTime();
            Windesheart.ConnectedDevice.FetchData(startDate.AddMinutes(1), FillDatabase);
            Globals.HomePageViewModel.IsLoading = false;
        }

        private async void FillDatabase(List<ActivitySample> samples)
        {
            Debug.WriteLine("Filling DB with samples");
            foreach (var sample in samples)
            {
                var datetime = sample.Timestamp;

                await AddHeartrate(datetime, sample);
                await AddStep(datetime, sample);
                await AddSleep(datetime, sample);
            }
            _heartrateRepository.SaveChangesAsync();
            _stepsRepository.SaveChangesAsync();
            _sleepRepository.SaveChangesAsync();
            Debug.WriteLine("Fetched all samples");
        }

        private async Task<DateTime> GetLastAddedDateTime()
        {
            var steps = await _stepsRepository.GetAllAsync();

            if (steps.Count() > 0)
            {
                Debug.WriteLine("Last added datetime is: " + steps.Last().DateTime);
                return steps.Last().DateTime;
            }
            Debug.WriteLine("Last added datetime is: " + DateTime.Now.AddDays(-30));
            return DateTime.Now.AddDays(-30);
        }

        private async Task AddHeartrate(DateTime datetime, ActivitySample sample)
        {
            var heartRate = new Heartrate(datetime, sample.HeartRate);
            await _heartrateRepository.AddAsync(heartRate);
        }

        private async Task AddStep(DateTime datetime, ActivitySample sample)
        {
            var step = new Step(datetime, sample.Steps);
            await _stepsRepository.AddAsync(step);
        }

        private async Task AddSleep(DateTime datetime, ActivitySample sample)
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

            await _sleepRepository.AddAsync(sleep);
        }

        public void EmptyDatabase()
        {
            _heartrateRepository.RemoveAll();
            _stepsRepository.RemoveAll();
            _sleepRepository.RemoveAll();
        }
    }
}
