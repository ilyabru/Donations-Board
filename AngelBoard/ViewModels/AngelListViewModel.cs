using AngelBoard.Models;
using AngelBoard.Services;
using AngelBoard.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AngelBoard.ViewModels
{
    public class AngelListViewModel : BaseViewModel
    {
        private readonly IAngelService _angelService;

        private ObservableCollection<Angel> angels;
        private Angel selectedAngel;
        private Angel inputAngel;

        private bool isNew = true;

        public AngelListViewModel()
        {
            _angelService = new AngelService();
            InputAngel = new Angel();
        }

        //private async Task<AngelListViewModel> InitializeAsync()
        //{
        //    Angels = 
        //}

        public AngelListViewModel(IAngelService angelService)
        {
            _angelService = angelService;
        }

        public ObservableCollection<Angel> Angels
        {
            get { return angels; }
            set { SetPropertyValue(ref angels, value); }
        }
        public Angel SelectedAngel
        {
            get { return selectedAngel; }
            set
            {
                SetPropertyValue(ref selectedAngel, value);
                RaisePropertyChanged(nameof(IsAngelSelected));
            }
        }

        public bool IsAngelSelected
        {
            get { return SelectedAngel != null; }
        }

        public Angel InputAngel
        {
            get { return inputAngel; }
            set { SetPropertyValue(ref inputAngel, value); }
        }

        public bool IsNew
        {
            get => isNew;
            set { SetPropertyValue(ref isNew, value); }
        }

        public ICommand SaveAngel => new RelayCommand(async () => await OnSaveAngel());
        public ICommand EditAngel => new RelayCommand(() => OnEditAngel());
        public ICommand CancelEditAngel => new RelayCommand(OnCancelEditAngel);
        public ICommand DeleteAngel => new RelayCommand(async () => await OnDeleteAngel());

        public override Task InitializeAsync(object navigationData)
        {
            IsNew = true;

            IsBusy = true;

            //Angels = await _angelService.GetAngelsAsync(); // TODO replace with navdata from main page?
            Angels = (ObservableCollection<Angel>)navigationData;
            InputAngel = new Angel();

            IsBusy = false;

            return base.InitializeAsync(navigationData);
        }

        private async Task OnSaveAngel()
        {
            if (IsNew)
            {
                await _angelService.AddAngelAsync(InputAngel);
                Angels.Add(InputAngel);

            }
            else
            {
                SelectedAngel.Name = InputAngel.Name;
                SelectedAngel.Location = InputAngel.Location;
                SelectedAngel.Amount = InputAngel.Amount;

                await _angelService.UpdateAngelAsync(InputAngel);

                SelectedAngel = null; // reset listview
            }

            // Clear textboxes
            IsNew = true;
            InputAngel = new Angel();
        }

        private void OnEditAngel()
        {
            IsNew = false;
            InputAngel = (Angel)SelectedAngel.Clone();
        }

        private void OnCancelEditAngel()
        {
            IsNew = true;
            InputAngel = new Angel();
        }

        private async Task OnDeleteAngel()
        {
            await _angelService.DeleteAngelAsync(SelectedAngel);
            Angels.Remove(SelectedAngel);
        }
    }
}
