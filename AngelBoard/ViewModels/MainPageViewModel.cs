using AngelBoard.Models;
using AngelBoard.Services;
using AngelBoard.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AngelBoard.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private IAngelService _angelService;
        private ISponsorService _sponsorService;

        private ObservableCollection<Angel> angels;
        private Sponsor selectedSponsor;

        public MainPageViewModel()
        {
            _angelService = new AngelService();

            Initialize();
        }

        private async Task<MainPageViewModel> InitializeAsync()
        {
            Angels = await _angelService.GetAngelsAsync();
            return this;
        }

        public static Task<MainPageViewModel> CreateAsync()
        {
            var ret = new MainPageViewModel();
            return ret.InitializeAsync();
        }

        public ObservableCollection<Angel> Angels
        {
            get => angels;
            set => SetPropertyValue(ref angels, value);
        }


        public Sponsor SelectedSponsor
        {
            get => selectedSponsor;
            set => SetPropertyValue(ref selectedSponsor, value);
        }


        //public ICommand EditAngels => new Command(async () => await OnEditAngels());
        //public ICommand ItemTapped => new Command<Angel>(async (a) => await OnItemTapped(a));

        public MainPageViewModel(IAngelService angelService,
            ISponsorService sponsorService)
        {
            _angelService = angelService;
            _sponsorService = sponsorService;

            //MessagingCenter.Subscribe<SponsorViewModel, Sponsor>(this, "changeSponsor", (sender, arg) =>
            //{
            //    SelectedSponsor = arg;
            //});
        }

        public async void Initialize()
        {
            IsBusy = true;

            Angels = await _angelService.GetAngelsAsync();
            //SelectedSponsor =  await _sponsorService.GetSponsorAsync(Preferences.Get("selected_sponsor", -1));
            //Preferences.Clear();

            IsBusy = false;
        }

        private async Task OnEditAngels()
        {
            //await NavigationService.NavigateToAsync<AngelListViewModel>(Angels);
        }

        private async Task OnItemTapped(Angel a)
        {
            //await NavigationService.NavigateToPopupAsync<AngelCarouselViewModel>(Angels);
        }
    }
}
