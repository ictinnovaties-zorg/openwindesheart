using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Data.Interfaces;

namespace WindesHeartApp.Data.Repository
{
    public class SampleRepository : ISampleRepository
    {
        private readonly DatabaseContext _databaseContext;
        public SampleRepository(string dbPath)
        {
            _databaseContext = new DatabaseContext(dbPath);
        }
        public Task<IEnumerable<Type>> GetSampleAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AddSampleAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> AddSamplesAsync()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveSamples()
        {
            throw new System.NotImplementedException();
        }
    }
}