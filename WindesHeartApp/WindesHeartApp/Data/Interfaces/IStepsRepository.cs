using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface IStepsRepository
    {
        Task<IEnumerable<Step>> GetAllAsync();
        Task<bool> AddAsync(Step steps);
        Task<bool> AddRangeAsync(List<Step> steps);
        void RemoveAll();
        void SaveChangesAsync();

    }
}
