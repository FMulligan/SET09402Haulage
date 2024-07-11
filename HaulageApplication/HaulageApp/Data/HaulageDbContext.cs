using SQLite;
using HaulageApp.Models;

namespace HaulageApp.Data
{
    public class HaulageDbContext
    {
        public SQLiteConnection connection;

        public HaulageDbContext() {}

        public void Init()
        {
            //initializing the SQLitePCL.raw library.
            SQLitePCL.Batteries.Init();
            
            string databasePath = Constants.DatabasePath;
            if (connection is not null)
            {
                return;
            }
            connection = new SQLiteConnection(databasePath, Constants.Flags);
            connection.CreateTable<Note>();
        }
    }
}