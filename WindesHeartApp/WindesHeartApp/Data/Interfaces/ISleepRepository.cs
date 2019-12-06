using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface ISleepRepository
    {
        Task<IEnumerable<Sleep>> GetAllAsync();
        Task<bool> AddAsync(Sleep sleep);
        void RemoveAll();
    }
}