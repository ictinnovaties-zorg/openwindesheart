using Microsoft.EntityFrameworkCore;
using WindesHeartApp.Models;

namespace WindesHeartApp.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Heartrate> Heartrates { get; set; }
        public DbSet<Step> Steps { get; set; }

        public DbSet<Sleep> Sleep { get; set; }

        private readonly string _databasePath;

        public DatabaseContext(string databasePath)
        {
            _databasePath = databasePath;
            //Database.EnsureDeleted();

            Database.EnsureCreated();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_databasePath}");
        }

       
    }
    public static class EntityExtensions
    {
        public static void Clear<T>(this DbSet<T> dbSet) where T : class
        {
            dbSet.RemoveRange(dbSet);
        }
    }
}
