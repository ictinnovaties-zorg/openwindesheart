
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Repository
{
    public class StepsRepository : IStepsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public StepsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<IEnumerable<Step>> GetAllAsync()
        {
            try
            {
                var steps = await _databaseContext.Steps.ToListAsync();
                return steps.OrderBy(x => x.DateTime).ToList();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> AddAsync(Step step)
        {
            try
            {
                var tracking = await _databaseContext.Steps.AddAsync(step);
                return tracking.State == EntityState.Added;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<bool> AddRangeAsync(List<Step> steps)
        {
            try
            {
                await _databaseContext.Steps.AddRangeAsync(steps);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        public void RemoveAll()
        {
            try
            {
                _databaseContext.Steps.Clear<Step>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not delete step entries: " + e);
            }
        }

        public async void SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
