using OpenWindesheartDemoApp.Models;
using System.Collections.Generic;

namespace OpenWindesheartDemoApp.Data.Interfaces
{
    public interface ISleepRepository
    {
        IEnumerable<Sleep> GetAll();
        void Add(Sleep sleep);
        void RemoveAll();
    }
}