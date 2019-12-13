using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface ISleepRepository
    {
        IEnumerable<Sleep> GetAll();
        void Add(Sleep sleep);
        void RemoveAll();
        IEnumerable<Sleep> HeartratesByQuery(Func<Sleep, bool> predicate);
    }
}