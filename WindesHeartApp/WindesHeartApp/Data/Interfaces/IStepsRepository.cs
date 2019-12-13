using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface IStepsRepository
    {
        IEnumerable<Step> GetAll();
        void Add(Step step);
        void RemoveAll();
        IEnumerable<Step> HeartratesByQuery(Func<Step, bool> predicate);

        DateTime LastAddedDatetime();
    }
}
