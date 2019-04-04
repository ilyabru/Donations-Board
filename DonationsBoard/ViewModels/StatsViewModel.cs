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
