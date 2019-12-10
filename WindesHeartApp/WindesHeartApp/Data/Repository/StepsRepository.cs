
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Repository
{
    public class StepsRepository : IStepsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public StepsRepository(string dbPath)
        {
            _databaseContext = new DatabaseContext(dbPath);
        }

        public async Task<IEnumerable<Step>> GetAllAsync()
        {
            try
            {
                var steps = await _databaseContext.Steps.ToListAsync();
                return steps;
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
                await _databaseContext.SaveChangesAsync();
                return tracking.State == EntityState.Added;
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
    }
}
