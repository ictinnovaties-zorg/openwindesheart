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
    public class HeartrateRepository : IHeartrateRepository
    {
        private readonly DatabaseContext _databaseContext;
        public HeartrateRepository(string dbPath)
        {
            _databaseContext = new DatabaseContext(dbPath);
        }

        public async Task<IEnumerable<Heartrate>> GetAllAsync()
        {
            try
            {
                var heartrates = await _databaseContext.Heartrates.ToListAsync();
                return heartrates;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<bool> AddAsync(Heartrate heartrate)
        {
            try
            {
                var tracking = await _databaseContext.Heartrates.AddAsync(heartrate);
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
                _databaseContext.Heartrates.Clear<Heartrate>();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Could not delete heartrate entries: " + e);
            }
        }

        public async Task<IEnumerable<Heartrate>> HeartratesByQueryAsync(Func<Heartrate, bool> predicate)
        {
            try
            {
                var heartrates = await _databaseContext.Heartrates.ToListAsync();
                return heartrates.Where(predicate);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public async void SaveChangesAsync()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}
