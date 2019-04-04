using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AngelBoard.Configuration;
using AngelBoard.Models;
using SQLite;

namespace AngelBoard.Services
{
    public class AngelService : IAngelService
    {
        private readonly ISQLiteService _sqliteService;
        private SQLiteAsyncConnection conn;

        public AngelService(ISQLiteService sqliteService)
        {
            _sqliteService = sqliteService;

            conn = _sqliteService.GetConnection("EasterSealsDonators.db");

            conn.CreateTableAsync<Donor>().Wait();
        }

        public async Task<ObservableCollection<Donor>> GetAngelsAsync()
        {
            var angels = await conn.Table<Donor>().Where(a => a.SessionId == AppSettings.Current.CurrentSession).ToListAsync();

            return new ObservableCollection<Donor>(angels);
        }

        public async Task AddAngelAsync(Donor angel)
        {
            angel.SessionId = AppSettings.Current.CurrentSession;

            await conn.InsertAsync(angel);
        }

        public async Task<Donor> GetAngelAsync(int angelId)
        {
            return await conn.Table<Donor>().FirstOrDefaultAsync(a => a.Id == angelId);
        }

        public async Task UpdateAngelAsync(Donor angel)
        {
            await conn.UpdateAsync(angel);
        }

        public async Task DeleteAngelAsync(Donor angel)
        {
            await conn.DeleteAsync(angel);
        }

        public async Task<ObservableCollection<string>> GetLocations()
        {
            var locations = await conn.QueryAsync<Donor>("SELECT DISTINCT Location FROM donators");

            return new ObservableCollection<string>(locations.Select(a => a.Location));
        }
    }
}
