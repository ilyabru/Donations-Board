using DonationsBoard.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AngelBoard.Services
{
    public interface IStatsService
    {
        Task<ObservableCollection<CityStatistic>> GetStatsAsync();
    }
}
