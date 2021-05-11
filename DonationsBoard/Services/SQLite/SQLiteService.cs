using DonationBoard.Helpers;
using SQLite;

namespace DonationBoard.Services
{
    public class SQLiteService : ISQLiteService
    {
        public SQLiteAsyncConnection GetConnection()
        {
            return new SQLiteAsyncConnection(FileAccessHelper.GetLocalFilePath("EasterSealsDonors.db"));
        }
    }
}
