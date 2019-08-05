using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DonationBoard.Configuration;
using DonationBoard.Models;
using SQLite;

namespace DonationBoard.Services
{
    public class StatsService : IStatsService
    {
        private readonly ISQLiteService _sqliteService;
        private SQLiteAsyncConnection conn;

        public StatsService(ISQLiteService sqliteService)
        {
            _sqliteService = sqliteService;

            conn = _sqliteService.GetConnection();
        }

        public async Task<ObservableCollection<CityStatistic>> GetStatsAsync()
        {
            var cityStats = await conn.QueryAsync<CityStatistic>(@"
                SELECT 
                Location as 'City',
                COUNT(Id) as 'TotalDonations',
                SUM(Amount) as 'AmountRaised',
                AVG(Amount) as 'AverageRaised'
                FROM donors
                WHERE SessionId = ?
                GROUP BY Location
                ORDER BY City, TotalDonations, AmountRaised
            ", AppSettings.Current.CurrentSession);

            return new ObservableCollection<CityStatistic>(cityStats);
        }
    }
}
