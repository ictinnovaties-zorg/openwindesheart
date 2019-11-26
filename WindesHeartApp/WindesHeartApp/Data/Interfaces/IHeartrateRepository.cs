using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface IHeartrateRepository
    {
        Task<IEnumerable<Heartrate>> GetHeartRatesAsync();

        Task<bool> AddHeartrateAsync(Heartrate heartrate);
        void RemoveHeartrates();
        Task<IEnumerable<Heartrate>> QueryProductsAsync(Func<Heartrate, bool> predicate);
    }
}