using AngelBoard.Helpers;
using SQLite;

namespace AngelBoard.Services
{
    public class SQLiteService : ISQLiteService
    {
        public SQLiteAsyncConnection GetConnection()
        {
            return new SQLiteAsyncConnection(FileAccessHelper.GetLocalFilePath("EasterSealsDonators.db"));
        }
    }
}
