
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Data.Models;
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

        public async Task<IEnumerable<StepsModel>> GetStepsAsync()
        {
            try
            {
                var stepInfos = await _databaseContext.Steps.ToListAsync();
                return stepInfos;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> AddStepsAsync(StepsModel steps)
        {
            try
            {
                var tracking = await _databaseContext.Steps.AddAsync(steps);
                await _databaseContext.SaveChangesAsync();
                var isAdded = tracking.State == EntityState.Added;
                return isAdded;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void RemoveSteps()
        {
            foreach (var stepinfo in _databaseContext.Steps)
                _databaseContext.Steps.Remove(stepinfo);
            _databaseContext.SaveChanges();
        }

        public async Task<IEnumerable<StepsModel>> StepsByQueryAsync(Func<StepsModel, bool> predicate)
        {
            try
            {
                var steps = _databaseContext.Steps.Where(predicate);
                return steps.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
