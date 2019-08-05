using SQLite;

namespace DonationBoard.Services
{
    public interface ISQLiteService
    {
        SQLiteAsyncConnection GetConnection();
    }
}
