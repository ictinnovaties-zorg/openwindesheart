
using System;
using System.Collections.Generic;
using System.Linq;
using WindesHeartApp.Data.Interfaces;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data.Repository
{
    public class StepsRepository : IStepsRepository
    {
        private readonly Database _database;

        public StepsRepository(Database database)
        {
            _database = database;
        }

        public void Add(Step step)
        {
            var query = "INSERT INTO Steps(DateTime, StepCount) VALUES(?,?)";
            var command = _database.Instance.CreateCommand(query, new object[] { step.DateTime, step.StepCount });
            command.ExecuteNonQuery();
        }

        public IEnumerable<Step> GetAll()
        {
            return _database.Instance.Table<Step>().OrderBy(x => x.DateTime).ToList();
        }

        public DateTime LastAddedDatetime()
        {
            var steps = this.GetAll();
            if (steps.Count() > 0)
            {
                return steps.Last().DateTime.AddMinutes(1);
            }

            return DateTime.Now.AddYears(-2);
        }

        public void RemoveAll()
        {
            var steps = this.GetAll();
            foreach (var step in steps)
            {
                var query = "DELETE FROM Steps WHERE Id = ?";
                var command = _database.Instance.CreateCommand(query, new object[] { step.Id });
                command.ExecuteNonQuery();
            }
        }
    }
}
