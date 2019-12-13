using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Data.Interfaces;

namespace WindesHeartApp.Data.Repository
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly Database _database;
        public SettingsRepository(Database database)
        {
            _database = database;
        }
        public Task<IEnumerable<string>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddAsync(string setting)
        {
            throw new NotImplementedException();
        }

        public void RemoveHeartrates()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> HeartratesByQueryAsync(Func<string, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}