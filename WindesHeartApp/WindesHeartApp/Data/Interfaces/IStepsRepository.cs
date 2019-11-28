using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface IStepsRepository
    {
        Task<IEnumerable<StepInfo>> GetStepInfoAsync();

        Task<bool> AddStepInfoAsync(StepInfo steps);
        void RemoveStepInfo();
        Task<IEnumerable<StepInfo>> QueryProductsAsync(Func<StepInfo, bool> predicate);
    }
}
