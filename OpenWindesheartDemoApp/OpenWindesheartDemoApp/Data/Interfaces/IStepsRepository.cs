using OpenWindesheartDemoApp.Models;
using System;
using System.Collections.Generic;

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
