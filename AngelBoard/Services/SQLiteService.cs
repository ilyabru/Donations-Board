using AngelBoard.Helpers;
using SQLite;

namespace AngelBoard.Services
{
    public class SQLiteService : ISQLiteService
    {
        public SQLiteAsyncConnection GetConnection(string dbPath)
        {
            return new SQLiteAsyncConnection(FileAccessHelper.GetLocalFilePath(dbPath));
        }
    }
}
