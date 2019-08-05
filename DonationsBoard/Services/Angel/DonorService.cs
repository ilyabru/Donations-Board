using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DonationBoard.Configuration;
using DonationBoard.Models;
using SQLite;

namespace DonationBoard.Services
{
    public class DonorService : IDonorService
    {
        private readonly ISQLiteService _sqliteService;
        private SQLiteAsyncConnection conn;

        public DonorService(ISQLiteService sqliteService)
        {
            _sqliteService = sqliteService;

            conn = _sqliteService.GetConnection();

            conn.CreateTableAsync<Donor>().Wait();
        }

        public async Task<ObservableCollection<Donor>> GetDonorsAsync()
        {
            var donors = await conn.Table<Donor>().Where(a => a.SessionId == AppSettings.Current.CurrentSession).ToListAsync();

            return new ObservableCollection<Donor>(donors);
        }

        public async Task AddDonorAsync(Donor donor)
        {
            donor.SessionId = AppSettings.Current.CurrentSession;

            await conn.InsertAsync(donor);
        }

        public async Task<Donor> GetDonorAsync(int donorId)
        {
            return await conn.Table<Donor>().FirstOrDefaultAsync(a => a.Id == donorId);
        }

        public async Task UpdateDonorAsync(Donor donor)
        {
            await conn.UpdateAsync(donor);
        }

        public async Task DeleteDonorAsync(Donor donor)
        {
            await conn.DeleteAsync(donor);
        }

        public async Task<ObservableCollection<string>> GetLocations()
        {
            var locations = await conn.QueryAsync<Donor>("SELECT DISTINCT Location FROM donors");

            return new ObservableCollection<string>(locations.Select(a => a.Location));
        }
    }
}
