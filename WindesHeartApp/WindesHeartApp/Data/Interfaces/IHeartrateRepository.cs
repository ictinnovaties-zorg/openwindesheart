using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface IHeartrateRepository
    {
        Task<IEnumerable<Heartrate>> GetAllAsync();
        Task<bool> AddAsync(Heartrate heartrate);

        void RemoveAll();

        Task<IEnumerable<Heartrate>> HeartratesByQueryAsync(Func<Heartrate, bool> predicate);
        
    }
}