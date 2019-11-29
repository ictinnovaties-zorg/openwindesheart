using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Data.Interfaces;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Data.Repository
{
    public class StepsRepository : IStepsRepository
    {
        private readonly DatabaseContext _databaseContext;
        public StepsRepository(string dbPath)
        {
            _databaseContext = new DatabaseContext(dbPath);
        }

        public Task<IEnumerable<StepInfo>> GetStepsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddStepsAsync(StepInfo steps)
        {
            throw new NotImplementedException();
        }

        public void RemoveSteps()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Heartrate>> StepsByQueryAsync(Func<Heartrate, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}