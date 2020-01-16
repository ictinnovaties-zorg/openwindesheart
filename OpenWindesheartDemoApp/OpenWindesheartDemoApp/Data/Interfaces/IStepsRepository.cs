using System;
using System.Collections.Generic;
using OpenWindesheartDemoApp.Models;

namespace OpenWindesheartDemoApp.Data.Interfaces
{
    public interface IStepsRepository
    {
        IEnumerable<Step> GetAll();
        void Add(Step step);
        void RemoveAll();
        DateTime LastAddedDatetime();
    }
}
