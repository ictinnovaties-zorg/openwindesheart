using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindesHeartApp.Data.Interfaces
{
    public interface ISampleRepository
    {
        Task<IEnumerable<Type>> GetSampleAsync();
        Task<bool> AddSampleAsync();
        Task<bool> AddSamplesAsync();
        void RemoveSamples();
    }
}