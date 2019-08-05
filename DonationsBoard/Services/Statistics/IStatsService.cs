using DonationBoard.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace DonationBoard.Services
{
    public interface IStatsService
    {
        Task<ObservableCollection<CityStatistic>> GetStatsAsync();
    }
}
