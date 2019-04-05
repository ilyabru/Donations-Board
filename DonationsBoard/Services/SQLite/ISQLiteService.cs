using SQLite;

namespace AngelBoard.Services
{
    public interface ISQLiteService
    {
        SQLiteAsyncConnection GetConnection();
    }
}
