using SQLite;
using System.IO;
using OpenWindesheartDemoApp.Models;
using OpenWindesheartDemoApp.Resources;

namespace OpenWindesheartDemoApp.Data
{
    public class Database
    {
        public string DbPath;

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
            DbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
                "WindesHeart.db");

            //Set Database
            Instance = new SQLiteConnection(DbPath);

            //Create the tables if not existing
            Instance.CreateTable<Heartrate>();
            Instance.CreateTable<Step>();
            Instance.CreateTable<Sleep>();
        }
    }
}
