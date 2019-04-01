using AngelBoard.Configuration;
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


            conn.CreateTableAsync<Angel>().Wait();
            InitializeData();
        }

        private void InitializeData()
        {
            // session ID will be 1 by default
            conn.InsertAllAsync(new List<Angel>
            {
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Ilya Brusnitsyn", Location = "Toronto", Amount = 250m, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Joanna Arnolds", Location = "Toronto", Amount = 1000m, IsMonthly = true, IsViewed = true },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Archie Eastwood", Location = "Milton", Amount = 333m, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Adrian Mead", Location = "Markham", Amount = 100m, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Susan Ward", Location = "Sarnia", Amount = 99.99m, IsViewed = true },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Donna Falcon Hampton + Rory Smithsson", Location = "Oakville", Amount = 100m, IsViewed = true },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Doug Walker", Location = "Markham", Amount = 100.00m, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Jim Edgar", Location = "Bolton", Amount = 666m, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Grace Moonwalker", Location = "Toronto", Amount = 100m, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Max Payne", Location = "New Jersey", Amount = 600m, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Pauline Fernandes", Location = "Guelph", Amount = 100m, IsMonthly = true, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Julia Shwartz", Location = "Scarborough", Amount = 150m, IsViewed = true },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "James Crawford", Location = "North Bay", Amount = 100m, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Gordon Freeman", Location = "City 17", Amount = 250m, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Wayne Newton", Location = "Markham", Amount = 100m, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Larry Gorrigan + Sue Maclean", Location = "Picton", Amount = 100m, IsViewed = false },
                new Angel { SessionId = AppSettings.Current.CurrentSession, Name = "Martine Martinez", Location = "Bolton", Amount = 110m, IsViewed = false },
            });
        }

        public async Task<ObservableCollection<Angel>> GetAngelsAsync()
        {
            var angels = await conn.Table<Angel>().Where(a => a.SessionId == AppSettings.Current.CurrentSession).ToListAsync();

            return new ObservableCollection<Angel>(angels);
        }

        public async Task AddAngelAsync(Angel angel)
        {
            angel.SessionId = AppSettings.Current.CurrentSession;

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

        public async Task<ObservableCollection<string>> GetLocations()
        {
            var locations = await conn.Table<Angel>().ToListAsync();

            return new ObservableCollection<string>(locations.Select(a => a.Location).Distinct().OrderBy(a => a));
        }
    }
}
