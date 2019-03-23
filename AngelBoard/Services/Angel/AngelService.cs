using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
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

            conn = _sqliteService.GetConnection("angels.db");

            conn.CreateTableAsync<Session>().Wait();
            //conn.InsertAsync(new Session { CreateDate = DateTime.Now }).Wait();
            conn.CreateTableAsync<Angel>().Wait();
        }

        public async Task<ObservableCollection<Angel>> GetAngelsAsync()
        {
            var angels = await conn.Table<Angel>().ToListAsync();

            return new ObservableCollection<Angel>(angels);
        }

        public async Task AddAngelAsync(Angel angel)
        {           
            await conn.InsertAsync(angel);
        }

        public async Task<Angel> GetAngelAsync(int angelId)
        {
            return await conn.Table<Angel>().Where(a => a.Id == angelId).FirstOrDefaultAsync();
        }

        public async Task UpdateAngelAsync(Angel angel)
        {
            await conn.UpdateAsync(angel);
        }

        public async Task DeleteAngelAsync(Angel angel)
        {
            await conn.DeleteAsync(angel);
        }
    }
}
