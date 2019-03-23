using AngelBoard.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelBoard.Services
{
    public class MockAngelService : IAngelService
    {
        private SQLiteAsyncConnection conn;

        public MockAngelService()
        {
            conn = new SQLiteAsyncConnection(":memory:");

            conn.CreateTableAsync<Session>().Wait();
            //conn.InsertAsync(new Session { CreateDate = DateTime.Now }).Wait();
            conn.CreateTableAsync<Angel>().Wait();

            InitializeData();
        }

        private void InitializeData()
        {
            conn.InsertAllAsync(new List<Angel>
            {
                new Angel { Name = "Ilya Brusnitsyn", Location = "Toronto", Amount = 250m, IsViewed = false },
                new Angel { Name = "Joanna Arnolds", Location = "Toronto", Amount = 1000m, IsViewed = true },
                new Angel { Name = "Archie Eastwood", Location = "Milton", Amount = 333m, IsViewed = false },
                new Angel { Name = "Adrian Mead", Location = "Markham", Amount = 100m, IsViewed = false },
                new Angel { Name = "Susan Ward", Location = "Sarnia", Amount = 99.99m, IsViewed = true },
                new Angel { Name = "Donna Falcon Hampton + Rory Smithsson", Location = "Oakville", Amount = 100m, IsViewed = true },
                new Angel { Name = "Doug Walker", Location = "Markham", Amount = 100.00m, IsViewed = false },
                new Angel { Name = "Jim Edgar", Location = "Bolton", Amount = 666m, IsViewed = false },
                new Angel { Name = "Grace Moonwalker", Location = "Toronto", Amount = 100m, IsViewed = false },
                new Angel { Name = "Max Payne", Location = "New Jersey", Amount = 600m, IsViewed = false },
                new Angel { Name = "Pauline Fernandes", Location = "Guelph", Amount = 100m, IsViewed = false },
                new Angel { Name = "Julia Shwartz", Location = "Scarborough", Amount = 150m, IsViewed = true },
                new Angel { Name = "James Crawford", Location = "North Bay", Amount = 100m, IsViewed = false },
                new Angel { Name = "Gordon Freeman", Location = "City 17", Amount = 250m, IsViewed = false },
                new Angel { Name = "Wayne Newton", Location = "Markham", Amount = 100m, IsViewed = false },
                new Angel { Name = "Larry Gorrigan + Sue Maclean", Location = "Picton", Amount = 100m, IsViewed = false },
                new Angel { Name = "Martine Martinez", Location = "Bolton", Amount = 110m, IsViewed = false },
            });
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
