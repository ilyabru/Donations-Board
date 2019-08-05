using DonationBoard.Services;
using DonationBoard.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DonationBoard.ViewModels
{
    public class ControlPanelViewModel : BaseViewModel
    {
        private readonly NavigationItem DonorListItem = new NavigationItem("Donor List", typeof(DonorListViewModel));
        private readonly NavigationItem StatsItem = new NavigationItem("Statistics", typeof(StatsViewModel));

        private INavigationService _navigationService;

        public ControlPanelViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private object _selectedItem;

        public object SelectedItem
        {
            get => _selectedItem;
            set => SetPropertyValue(ref _selectedItem,value);
        }


        private IEnumerable<NavigationItem> _items;
        public IEnumerable<NavigationItem> Items
        {
            get => _items;
            set => SetPropertyValue(ref _items, value);
        }

        public void LoadAsync()
        {
            Items = GetItems().ToArray();
            _navigationService.Navigate(typeof(DonorListViewModel));
        }

        public void NavigateTo(Type viewModel)
        {
            _navigationService.Navigate(viewModel);

            //switch (viewModel.Name)
            //{
            //    case nameof(AngelListViewModel):
            //        _navigationService.Navigate(viewModel);
            //        break;
            //    case nameof(SettingsViewModel):
            //        _navigationService.Navigate(viewModel);
            //        break;
            //    default:
            //        throw new NotImplementedException();
            //}
        }

        private IEnumerable<NavigationItem> GetItems()
        {
            yield return DonorListItem;
            yield return StatsItem;
        }
    }
}
