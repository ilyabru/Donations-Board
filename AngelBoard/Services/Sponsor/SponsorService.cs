using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using AngelBoard.Models;
using SQLite;

namespace AngelBoard.Services
{
    public class SponsorService : ISponsorService
    {
        private SQLiteAsyncConnection conn;

        public SponsorService()
        {
            conn = SQLiteService.GetConnection("angels.db");

            conn.CreateTableAsync<Sponsor>().Wait();
        }

        public async Task AddSponsorAsync(Sponsor sponsor)
        {
            await conn.InsertAsync(sponsor);
        }

        public async Task DeleteSponsorAsync(Sponsor sponsor)
        {
            await conn.DeleteAsync(sponsor);
        }

        public async Task<Sponsor> GetSponsorAsync(int sponsorId)
        {
            return await conn.Table<Sponsor>().Where(a => a.Id == sponsorId).FirstOrDefaultAsync();
        }

        public async Task<ObservableCollection<Sponsor>> GetSponsorsAsync()
        {
            var sponsors = await conn.Table<Sponsor>().ToListAsync();

            return new ObservableCollection<Sponsor>(sponsors);
        }
    }
}
