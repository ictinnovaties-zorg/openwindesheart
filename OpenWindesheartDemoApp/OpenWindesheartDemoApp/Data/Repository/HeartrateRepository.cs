using OpenWindesheartDemoApp.Data.Interfaces;
using OpenWindesheartDemoApp.Models;
using System.Collections.Generic;

namespace OpenWindesheartDemoApp.Data.Repository
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
