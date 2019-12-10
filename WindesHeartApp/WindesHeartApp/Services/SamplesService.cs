using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;
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

        public void StartFetching(DateTime startDate)
        {
            Windesheart.ConnectedDevice.FetchData(startDate, FillDatabase);
        }

        private async void FillDatabase(List<ActivitySample> samples)
        {
            foreach (var sample in samples)
            {
                var datetime = sample.Timestamp;
                await AddHeartrate(datetime, sample);
                await AddStep(datetime, sample);
                await AddSleep(datetime, sample);
            }
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
