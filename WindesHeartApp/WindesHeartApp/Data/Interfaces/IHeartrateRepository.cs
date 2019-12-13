using System.Collections.Generic;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Interfaces
{
    public interface IHeartrateRepository
    {
        IEnumerable<Heartrate> GetAll();
        void Add(Heartrate heartrate);
        void RemoveAll();
    }
}