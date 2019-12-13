using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Repository
{
    public class HeartrateRepository : IHeartrateRepository
    {
        private readonly Database _database;
        public HeartrateRepository(Database database)
        {
            _database = database;
        }

        public void Add(Heartrate heartrate)
        {
            var query = "INSERT INTO Heartrates(DateTime, HeartrateValue) VALUES(?,?)";
            var command = _database.Instance.CreateCommand(query, new object[] { heartrate.DateTime, heartrate.HeartrateValue });
            command.ExecuteNonQuery();
        }

        public IEnumerable<Heartrate> GetAll()
        {
            return _database.Instance.Table<Heartrate>().OrderBy(x => x.DateTime).ToList();
        }

        public IEnumerable<Heartrate> HeartratesByQuery(Func<Heartrate, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll()
        {
            var heartrates = this.GetAll();
            foreach (var heartrate in heartrates)
            {
                var query = "DELETE FROM Heartrates WHERE Id = ?";
                var command = _database.Instance.CreateCommand(query, new object[] { heartrate.Id });
                command.ExecuteNonQuery();
            }
        }
    }
}
