using AngelBoard.Helpers;
using SQLite;

namespace AngelBoard.Services
{
    static class SQLiteService
    {
        public static SQLiteAsyncConnection GetConnection(string dbPath)
        {
            return new SQLiteAsyncConnection(FileAccessHelper.GetLocalFilePath(dbPath));
        }
    }
}
