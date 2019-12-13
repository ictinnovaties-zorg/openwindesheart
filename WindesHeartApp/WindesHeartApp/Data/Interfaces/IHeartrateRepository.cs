using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface IHeartrateRepository
    {
        IEnumerable<Heartrate> GetAll();
        void Add(Heartrate heartrate);
        void RemoveAll();
        IEnumerable<Heartrate> HeartratesByQuery(Func<Heartrate, bool> predicate);
    }
}