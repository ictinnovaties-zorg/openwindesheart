using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindesHeartApp.Data.Interfaces
{
    public interface ISettingsRepository
    {
        Task<IEnumerable<string>> Settings();
        Task<bool> AddSetting(string setting);
        void RemoveHeartrates();
        Task<IEnumerable<string>> HeartratesByQueryAsync(Func<string, bool> predicate);
    }
}