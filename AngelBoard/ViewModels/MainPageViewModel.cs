using AngelBoard.Helpers;
using AngelBoard.Models;
using AngelBoard.Services;
using AngelBoard.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AngelBoard.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private IAngelService _angelService;
        private IContextService _contextService;
        private INavigationService _navigationService;
        private IMessageService _messageService;
        //private ISponsorService _sponsorService;

        private ObservableCollection<Angel> angels;
        private Angel selectedAngel;
        private Sponsor selectedSponsor;

        public MainPageViewModel(IAngelService angelService,
                                 IContextService contextService,
                                 INavigationService navigationService,
                                 IMessageService messageService)
        {
            _angelService = angelService;
            _contextService = contextService;
            _navigationService = navigationService;
            _messageService = messageService;

            Initialize();

        }

        //private async Task<MainPageViewModel> InitializeAsync()
        //{
        //    Angels = await _angelService.GetAngelsAsync();
        //    return this;
        //}

        //public static Task<MainPageViewModel> CreateAsync()
        //{
        //    var ret = new MainPageViewModel();
        //    return ret.InitializeAsync();
        //}

        public ObservableCollection<Angel> Angels
        {
            get => angels;
            set => SetPropertyValue(ref angels, value);
        }

        public Angel SelectedAngel
        {
            get => selectedAngel;
            set => SetPropertyValue(ref selectedAngel, value);
        }

        public Sponsor SelectedSponsor
        {
            get => selectedSponsor;
            set => SetPropertyValue(ref selectedSponsor, value);
        }


        public ICommand EditAngels => new RelayCommand(async () => await OnEditAngels());
        //public ICommand ItemTapped => new Command<Angel>(async (a) => await OnItemTapped(a));
        public ICommand SelectionChanged => new RelayCommand<(IList<object>, IList<object>)>(async (s) => await OnSelectionChanged(s));

        public async void Initialize()
        {
            IsBusy = true;


            Angels = await _angelService.GetAngelsAsync();
            //SelectedSponsor =  await _sponsorService.GetSponsorAsync(Preferences.Get("selected_sponsor", -1));
            //Preferences.Clear();

            IsBusy = false;
        }

        public void Subscribe()
        {
            _messageService.Subscribe<AngelListViewModel, Angel>(this, OnAngelSaved);
        }

        private async void OnAngelSaved(AngelListViewModel sender, string message, Angel changed)
        {
            switch (message)
            {
                case "NewAngelSaved":
                    await _contextService.RunAsync(() =>
                    {
                        Angels.Add((Angel)changed.Clone());
                    });
                    break;
                case "AngelChanged":
                    await _contextService.RunAsync(() =>
                    {
                        var angelIndex = Angels.IndexOf(Angels.Where(a => a.Id == changed.Id).First());
                        Angels[angelIndex] = (Angel)changed.Clone();
                    });
                    break;
            }
        }

        private void AddEditAngel(Angel angel)
        {
            int index = Angels.IndexOf(Angels.Where(a => a.Id == angel.Id).First());
            Angels[index] = angel;
        }

        private async Task OnEditAngels()
        {
            await _navigationService.CreateNewViewAsync<ControlPanelViewModel>(Angels);
        }

        private async Task OnItemTapped(Angel a)
        {
            //await NavigationService.NavigateToPopupAsync<AngelCarouselViewModel>(Angels);
        }

        private Task OnSelectionChanged((IList<object>, IList<object>) changedObjects)
        {
            var changedAngels = (newItem: (Angel)changedObjects.Item1.FirstOrDefault(),
                oldItem: (Angel)changedObjects.Item2.FirstOrDefault());

            if (changedAngels.newItem != null && changedAngels.oldItem != null)
            {
                if (changedAngels.oldItem.IsViewed == false)
                {
                    changedAngels.oldItem.IsViewed = true;
                }
            }

            return Task.FromResult(false);
        }
    }
}
