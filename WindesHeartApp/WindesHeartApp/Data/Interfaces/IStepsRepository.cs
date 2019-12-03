using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Data.Models;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface IStepsRepository
    {
        Task<IEnumerable<StepsModel>> GetStepsAsync();
        Task<bool> AddStepsAsync(StepsModel steps);
        void RemoveSteps();
        Task<IEnumerable<StepsModel>> StepsByQueryAsync(Func<StepsModel, bool> predicate);
    }
}
