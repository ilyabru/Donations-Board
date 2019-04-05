﻿using AngelBoard.Configuration;
using AngelBoard.Models;
using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AngelBoard.Services
{
    public class MockAngelService : IAngelService
    {
        private readonly ISQLiteService _sqliteService;
        private SQLiteAsyncConnection conn;

        public MockAngelService(ISQLiteService sqliteService)
        {
            _sqliteService = sqliteService;

            conn = _sqliteService.GetConnection();

            conn.CreateTableAsync<Donor>().Wait();
            InitializeData();
        }

        private void InitializeData()
        {
            // session ID will be 1 by default
            conn.InsertAllAsync(new List<Donor>
            {
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Ilya Brusnitsyn", Location = "Toronto", Amount = 250m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Joanna Arnolds", Location = "Toronto", Amount = 1000m, IsMonthly = true, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Archie Eastwood", Location = "Milton", Amount = 333m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Adrian Mead", Location = "Markham", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Susan Ward", Location = "Sarnia", Amount = 99.99m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Donna Falcon Hampton + Rory Smithsson", Location = "Oakville", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Doug Walker", Location = "Markham", Amount = 100.00m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Jim Edgar", Location = "Bolton", Amount = 666m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Grace Moonwalker", Location = "Toronto", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Max Payne", Location = "New Jersey", Amount = 600m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Pauline Fernandes", Location = "Guelph", Amount = 100m, IsMonthly = true, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Julia Shwartz", Location = "Scarborough", Amount = 150m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "James Crawford", Location = "North Bay", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Gordon Freeman", Location = "City 17", Amount = 250m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Wayne Newton", Location = "Markham", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Larry Gorrigan + Sue Maclean", Location = "Picton", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Martine Martinez", Location = "Bolton", Amount = 110m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Max Payne", Location = "New Jersey", Amount = 600m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Pauline Fernandes", Location = "Guelph", Amount = 100m, IsMonthly = true, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Julia Shwartz", Location = "Scarborough", Amount = 150m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "James Crawford", Location = "North Bay", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Gordon Freeman", Location = "City 17", Amount = 250m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Wayne Newton", Location = "Markham", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Larry Gorrigan + Sue Maclean", Location = "Picton", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Martine Martinez", Location = "Bolton", Amount = 110m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Ilya Brusnitsyn", Location = "Toronto", Amount = 250m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Joanna Arnolds", Location = "Toronto", Amount = 1000m, IsMonthly = true, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Archie Eastwood", Location = "Milton", Amount = 333m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Adrian Mead", Location = "Markham", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Susan Ward", Location = "Sarnia", Amount = 99.99m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Donna Falcon Hampton + Rory Smithsson", Location = "Oakville", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Doug Walker", Location = "Markham", Amount = 100.00m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Jim Edgar", Location = "Bolton", Amount = 666m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Grace Moonwalker", Location = "Toronto", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Max Payne", Location = "New Jersey", Amount = 600m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Pauline Fernandes", Location = "Guelph", Amount = 100m, IsMonthly = true, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Julia Shwartz", Location = "Scarborough", Amount = 150m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "James Crawford", Location = "North Bay", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Gordon Freeman", Location = "City 17", Amount = 250m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Wayne Newton", Location = "Markham", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Larry Gorrigan + Sue Maclean", Location = "Picton", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Martine Martinez", Location = "Bolton", Amount = 110m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Max Payne", Location = "New Jersey", Amount = 600m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Pauline Fernandes", Location = "Guelph", Amount = 100m, IsMonthly = true, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Julia Shwartz", Location = "Scarborough", Amount = 150m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "James Crawford", Location = "North Bay", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Gordon Freeman", Location = "City 17", Amount = 250m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Wayne Newton", Location = "Markham", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Larry Gorrigan + Sue Maclean", Location = "Picton", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Martine Martinez", Location = "Bolton", Amount = 110m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Wayne Newton", Location = "Markham", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Larry Gorrigan + Sue Maclean", Location = "Picton", Amount = 100m, IsViewed = false },
                new Donor { SessionId = AppSettings.Current.CurrentSession, Name = "Martine Martinez", Location = "Bolton", Amount = 110m, IsViewed = false },
            });
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
            return await conn.Table<Donor>().Where(a => a.Id == angelId).FirstOrDefaultAsync();
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
            var locations = await conn.QueryAsync<Donor>("SELECT DISTINCT Location FROM donors");

            return new ObservableCollection<string>(locations.Select(a => a.Location));
        }
    }
}
