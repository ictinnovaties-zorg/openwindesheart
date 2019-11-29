using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface ISleepRepository
    {
        Task<IEnumerable<Heartrate>> GetSleepAsync();
        Task<bool> AddSleepAsync(Type sleep);
        void RemoveSleep();
        Task<IEnumerable<Heartrate>> SleepByQueryAsync(Func<Heartrate, bool> predicate);
    }
}