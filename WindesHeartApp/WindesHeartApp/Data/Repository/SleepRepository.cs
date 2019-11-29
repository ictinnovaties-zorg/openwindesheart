using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Data.Interfaces;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Data.Repository
{
    public class SleepRepository : ISleepRepository
    {
        private readonly DatabaseContext _databaseContext;
        public SleepRepository(string dbPath)
        {
            _databaseContext = new DatabaseContext(dbPath);
        }

        public Task<IEnumerable<Heartrate>> GetSleepAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddSleepAsync(Type sleep)
        {
            throw new NotImplementedException();
        }

        public void RemoveSleep()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Heartrate>> SleepByQueryAsync(Func<Heartrate, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}