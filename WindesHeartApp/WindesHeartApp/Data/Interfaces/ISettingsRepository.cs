using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindesHeartApp.Data.Interfaces
{
    public interface ISettingsRepository
    {
        Task<IEnumerable<string>> GetAllAsync();
        Task<bool> AddAsync(string setting);
    }
}