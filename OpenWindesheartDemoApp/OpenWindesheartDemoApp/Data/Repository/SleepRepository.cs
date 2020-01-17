using OpenWindesheartDemoApp.Data.Interfaces;
using OpenWindesheartDemoApp.Models;
using System.Collections.Generic;

namespace OpenWindesheartDemoApp.Data.Repository
{
    public class SleepRepository : ISleepRepository
    {
        private readonly Database _database;
        public SleepRepository(Database database)
        {
            _database = database;
        }

        public void Add(Sleep sleep)
        {
            var query = "INSERT INTO Sleep(DateTime, SleepType) VALUES(?,?)";
            var command = _database.Instance.CreateCommand(query, sleep.DateTime, sleep.SleepType);
            command.ExecuteNonQuery();
        }

        public IEnumerable<Sleep> GetAll()
        {
            return _database.Instance.Table<Sleep>().OrderBy(x => x.DateTime).ToList();
        }

        public void RemoveAll()
        {
            var sleeps = GetAll();
            foreach (var sleep in sleeps)
            {
                var query = "DELETE FROM Sleep WHERE Id = ?";
                var command = _database.Instance.CreateCommand(query, sleep.Id);
                command.ExecuteNonQuery();
            }
        }
    }
}