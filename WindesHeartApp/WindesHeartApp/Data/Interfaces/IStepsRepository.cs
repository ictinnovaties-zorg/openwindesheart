using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface IStepsRepository
    {
        Task<IEnumerable<StepInfo>> GetStepsAsync();
        Task<bool> AddStepsAsync(StepInfo steps);
        void RemoveSteps();
        Task<IEnumerable<Heartrate>> StepsByQueryAsync(Func<Heartrate, bool> predicate);
    }
}
