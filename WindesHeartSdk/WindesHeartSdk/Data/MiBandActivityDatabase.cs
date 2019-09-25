using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using WindesHeartSdk.Model;

namespace WindesHeartSdk.Data
{
    public class MiBandActivityDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public MiBandActivityDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<MiBandActivitySample>().Wait();
        }

        public async Task<int> InsertItemsAsync(List<MiBandActivitySample> items)
        {
            return await _database.InsertAllAsync(items, false);
        }

        public async Task<MiBandActivitySample> GetLastSample()
        {
            return await _database.Table<MiBandActivitySample>().OrderByDescending(x => x.Timestamp).FirstOrDefaultAsync();
        }

        public async Task<List<MiBandActivitySample>> GetTodaySamples()
        {
            var today = DateTime.Today.ToUniversalTime();
            return await _database.Table<MiBandActivitySample>().Where(x => x.Timestamp > today).ToListAsync();
        }

        public async Task<List<MiBandActivitySample>> GetSamplesSince(DateTime dateTime)
        {
            var samples = await _database.Table<MiBandActivitySample>().Where(x => x.Timestamp > dateTime && x.HeartRate < 255).ToListAsync();
            return samples.Where(x => x.Timestamp.Minute % 5 == 0).ToList();
        }

        public Task<int> DeleteAllAsync()
        {
            return _database.DeleteAllAsync<MiBandActivitySample>();
        }
    }
}
