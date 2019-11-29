using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WindesHeartApp.Data.Interfaces;
using WindesHeartSDK.Models;

namespace WindesHeartApp.Data.Repository
{
    public class HeartrateRepository : IHeartrateRepository
    {
        private readonly DatabaseContext _databaseContext;
        public HeartrateRepository(string dbPath)
        {
            _databaseContext = new DatabaseContext(dbPath);
        }


        public async Task<IEnumerable<Heartrate>> GetHeartRatesAsync()
        {
            try
            {
                var heartrates = await _databaseContext.Heartrates.ToListAsync();
                return heartrates;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> AddHeartrateAsync(Heartrate heartrate)
        {
            try
            {
                var tracking = await _databaseContext.Heartrates.AddAsync(heartrate);
                await _databaseContext.SaveChangesAsync();
                var isAdded = tracking.State == EntityState.Added;
                return isAdded;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public void RemoveHeartrates()
        {
            foreach (var heartrate in _databaseContext.Heartrates)
                _databaseContext.Heartrates.Remove(heartrate);
            _databaseContext.SaveChanges();
        }

        public async Task<IEnumerable<Heartrate>> HeartratesByQueryAsync(Func<Heartrate, bool> predicate)
        {
            try
            {
                var heartrates = _databaseContext.Heartrates.Where(predicate);
                return heartrates.ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
