using System;
using System.IO;
using WindesHeartSdk.Data;

namespace WindesHeartSdk.MiBand
{
    public static class MiBandDb
    {
        private static MiBandActivityDatabase _database;

        public static MiBandActivityDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new MiBandActivityDatabase(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MiBandActivity.db3"));
                }

                return _database;
            }
        }
    }
}
