using System;
using System.Collections.Generic;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface IStepsRepository
    {
        IEnumerable<Step> GetAll();
        void Add(Step step);
        void RemoveAll();
        DateTime LastAddedDatetime();
    }
}
