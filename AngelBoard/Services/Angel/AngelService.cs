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
        private SQLiteAsyncConnection conn;

        public AngelService()
        {
            conn = SQLiteService.GetConnection("angels.db");

            conn.CreateTableAsync<Session>().Wait();
            //conn.InsertAsync(new Session { CreateDate = DateTime.Now }).Wait();
            conn.CreateTableAsync<Angel>().Wait();
        }

        //basic validation to ensure values were entered
        private void ValidateAngel(Angel angel)
        {
            if (string.IsNullOrEmpty(angel.Name))
                throw new Exception("Valid name required");

            if (string.IsNullOrEmpty(angel.Location))
                throw new Exception("Valid location required");

            if (string.IsNullOrEmpty(angel.Amount))
                throw new Exception("Valid amount required");
        }

        public async Task<ObservableCollection<Angel>> GetAngelsAsync()
        {
            var angels = await conn.Table<Angel>().ToListAsync();

            return new ObservableCollection<Angel>(angels);
        }

        public async Task AddAngelAsync(Angel angel)
        {
            ValidateAngel(angel);
            
            await conn.InsertAsync(angel);
        }

        public async Task<Angel> GetAngelAsync(int angelId)
        {
            return await conn.Table<Angel>().Where(a => a.Id == angelId).FirstOrDefaultAsync();
        }

        public async Task UpdateAngelAsync(Angel angel)
        {
            ValidateAngel(angel);

            await conn.UpdateAsync(angel);
        }

        public async Task DeleteAngelAsync(Angel angel)
        {
            await conn.DeleteAsync(angel);
        }
    }
}
