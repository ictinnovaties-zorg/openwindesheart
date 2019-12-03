using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Data.Models;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface IHeartrateRepository
    {
        Task<IEnumerable<HeartrateModel>> GetHeartRatesAsync();
        Task<bool> AddHeartrateAsync(HeartrateModel heartrate);
        void RemoveHeartrates();
        Task<IEnumerable<HeartrateModel>> HeartratesByQueryAsync(Func<HeartrateModel, bool> predicate);
    }
}