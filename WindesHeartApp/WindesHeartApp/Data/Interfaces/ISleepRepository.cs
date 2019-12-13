using System.Collections.Generic;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface ISleepRepository
    {
        IEnumerable<Sleep> GetAll();
        void Add(Sleep sleep);
        void RemoveAll();
    }
}