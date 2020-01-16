using System.Collections.Generic;
using OpenWindesheartDemoApp.Models;

namespace OpenWindesheartDemoApp.Data.Interfaces
{
    public interface IHeartrateRepository
    {
        IEnumerable<Heartrate> GetAll();
        void Add(Heartrate heartrate);
        void RemoveAll();
    }
}