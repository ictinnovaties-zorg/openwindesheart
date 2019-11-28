using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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


        public async Task<IEnumerable<StepInfo>> GetStepInfoAsync()
        {
            try
            {
                var stepInfos = await _databaseContext.Stepinfo.ToListAsync();
                return stepInfos;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> AddStepInfoAsync(StepInfo stepinfo)
        {
            try
            {
                var tracking = await _databaseContext.Stepinfo.AddAsync(stepinfo);
                await _databaseContext.SaveChangesAsync();
                var isAdded = tracking.State == EntityState.Added;
                return isAdded;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void RemoveStepInfo()
        {
            foreach (var stepinfo in _databaseContext.Stepinfo)
                _databaseContext.Stepinfo.Remove(stepinfo);
            _databaseContext.SaveChanges();
        }

        public async Task<IEnumerable<StepInfo>> QueryProductsAsync(Func<StepInfo, bool> predicate)
        {
            try
            {
                var stepinfos = _databaseContext.Stepinfo.Where(predicate);
                return stepinfos.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
