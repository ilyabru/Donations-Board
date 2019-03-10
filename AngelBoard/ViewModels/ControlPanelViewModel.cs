using AngelBoard.Services;
using AngelBoard.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelBoard.ViewModels
{
    public class ControlPanelViewModel : BaseViewModel
    {
        private readonly NavigationItem AngelListItem = new NavigationItem("Angel List", typeof(AngelListViewModel));

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
            _navigationService.Navigate(typeof(AngelListViewModel));
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
            yield return AngelListItem;
        }
    }
}
