using OpenWindesheartDemoApp.Data.Interfaces;
using OpenWindesheartDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenWindesheartDemoApp.Data.Repository
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
            var command = _database.Instance.CreateCommand(query, step.DateTime, step.StepCount);
            command.ExecuteNonQuery();
        }

        public IEnumerable<Step> GetAll()
        {
            return _database.Instance.Table<Step>().OrderBy(x => x.DateTime).ToList();
        }

        public DateTime LastAddedDatetime()
        {
            var steps = GetAll().ToArray();
            return steps.Any() ? steps.Last().DateTime.AddMinutes(1) : DateTime.Now.AddYears(-2);
        }

        public void RemoveAll()
        {
            var steps = GetAll();
            foreach (var step in steps)
            {
                var query = "DELETE FROM Steps WHERE Id = ?";
                var command = _database.Instance.CreateCommand(query, step.Id);
                command.ExecuteNonQuery();
            }
        }
    }
}
