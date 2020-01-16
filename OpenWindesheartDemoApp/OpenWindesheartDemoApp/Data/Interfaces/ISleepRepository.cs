using System.Collections.Generic;
using OpenWindesheartDemoApp.Models;

namespace OpenWindesheartDemoApp.Data.Interfaces
{
    public interface ISleepRepository
    {
        IEnumerable<Sleep> GetAll();
        void Add(Sleep sleep);
        void RemoveAll();
    }
}