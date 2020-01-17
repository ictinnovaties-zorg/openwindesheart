using OpenWindesheartDemoApp.Models;
using OpenWindesheartDemoApp.Resources;
using SQLite;
using System.IO;

namespace OpenWindesheartDemoApp.Data
{
    public class Database
    {
        private string _dbPath;

        public SQLiteConnection Instance;

        public Database()
        {
            CreateDatabase();
        }

        public void EmptyDatabase()
        {
            //Transaction for emptying DB-data
            Instance.BeginTransaction();
            Globals.HeartrateRepository.RemoveAll();
            Globals.StepsRepository.RemoveAll();
            Globals.SleepRepository.RemoveAll();
            Instance.Commit();
        }

        private void CreateDatabase()
        {
            //Set DbPath
            _dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                "WindesHeart.db");

            //Set Database
            Instance = new SQLiteConnection(_dbPath);

            //Create the tables if not existing
            Instance.CreateTable<Heartrate>();
            Instance.CreateTable<Step>();
            Instance.CreateTable<Sleep>();
        }
    }
}
