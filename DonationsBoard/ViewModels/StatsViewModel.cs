using AngelBoard.Services;
using AngelBoard.ViewModels.Base;
using DonationsBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelBoard.ViewModels
{
    public class StatsViewModel : BaseViewModel
    {
        private readonly IStatsService _statsService;
        private readonly IMessageService _messageService;

        private ObservableCollection<CityStatistic> cityStatistics;
        private string _cachedSortedColumn;

        public StatsViewModel(IStatsService statsService, IMessageService messageService)
        {
            _statsService = statsService;
            _messageService = messageService;
        }

        public ObservableCollection<CityStatistic> CityStatistics
        {
            get { return cityStatistics; }
            set { SetPropertyValue(ref cityStatistics, value); }
        }

        public string CachedSortedColumn
        {
            get => _cachedSortedColumn;
            set => _cachedSortedColumn = value;
        }

        public async Task LoadAsync()
        {
            IsBusy = true;

            await RefreshAsync();

            IsBusy = false;
        }

        public async Task RefreshAsync()
        {
            CityStatistics = await _statsService.GetStatsAsync();
        }

        public ObservableCollection<CityStatistic> SortData(string sortBy, bool ascending)
        {
            _cachedSortedColumn = sortBy;
            switch (sortBy)
            {
                case "City":
                    if (ascending)
                    {
                        return new ObservableCollection<CityStatistic>(from city in cityStatistics
                                                                       orderby city.City ascending
                                                                       select city);
                    }
                    else
                    {
                        return new ObservableCollection<CityStatistic>(from city in cityStatistics
                                                                       orderby city.City descending
                                                                       select city);
                    }
                case "TotalDonations":
                    if (ascending)
                    {
                        return new ObservableCollection<CityStatistic>(from city in cityStatistics
                                                                       orderby city.TotalDonations ascending
                                                                       select city);
                    }
                    else
                    {
                        return new ObservableCollection<CityStatistic>(from city in cityStatistics
                                                                       orderby city.TotalDonations descending
                                                                       select city);
                    }
                case "AmountRaised":
                    if (ascending)
                    {
                        return new ObservableCollection<CityStatistic>(from city in cityStatistics
                                                                       orderby city.AmountRaised ascending
                                                                       select city);
                    }
                    else
                    {
                        return new ObservableCollection<CityStatistic>(from city in cityStatistics
                                                                       orderby city.AmountRaised descending
                                                                       select city);
                    }
                case "AverageRaised":
                    if (ascending)
                    {
                        return new ObservableCollection<CityStatistic>(from city in cityStatistics
                                                                       orderby city.AverageRaised ascending
                                                                       select city);
                    }
                    else
                    {
                        return new ObservableCollection<CityStatistic>(from city in cityStatistics
                                                                       orderby city.AverageRaised descending
                                                                       select city);
                    }
            }

            return cityStatistics;
        }

        public ObservableCollection<CityStatistic> ResetData()
        {
            return new ObservableCollection<CityStatistic>(cityStatistics);
        }

        //public void Subscribe()
        //{
        //    _messageService.Subscribe<StatsViewModel, bool>(this, OnDonorUpdated);
        //}

        //public void Unsubscribe()
        //{
        //    _messageService.Unsubscribe(this);
        //}

        //private async void OnDonorUpdated(StatsViewModel arg1, string arg2, bool arg3)
        //{
        //    await RefreshAsync();
        //}
    }
}
