using OpenWindesheartDemoApp.Models;
using System.Collections.Generic;

namespace OpenWindesheartDemoApp.Data.Interfaces
{
    public interface IHeartrateRepository
    {
        IEnumerable<Heartrate> GetAll();
        void Add(Heartrate heartrate);
        void RemoveAll();
    }
}