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
        private bool isViewing = false;

        public MainPageViewModel(IAngelService angelService,
                                 IContextService contextService,
                                 INavigationService navigationService,
                                 IMessageService messageService)
        {
            _angelService = angelService;
            _contextService = contextService;
            _navigationService = navigationService;
            _messageService = messageService;
        }

        public ObservableCollection<Angel> Angels
        {
            get => angels;
            set => SetPropertyValue(ref angels, value);
        }

        public Angel SelectedAngel
        {
            get => selectedAngel;
            set
            {
                // sets isViewed property when navigating the flipview
                if (selectedAngel != null &&
                    selectedAngel != value &&
                    value != null && 
                    isViewing == true)
                {
                    SelectedAngel.IsViewed = true;
                }

                SetPropertyValue(ref selectedAngel, value);
            }
        }


        public bool IsViewing
        {
            get => isViewing;
            set
            {
                // sets isViewed property when leaving flipview
                if (isViewing == true && 
                    value == false &&
                    SelectedAngel != null)
                {
                    SelectedAngel.IsViewed = true;
                }

                SetPropertyValue(ref isViewing, value);
            }
        }

        public Sponsor SelectedSponsor
        {
            get => selectedSponsor;
            set => SetPropertyValue(ref selectedSponsor, value);
        }

        public ICommand EditAngels => new RelayCommand(async () => await OnEditAngels());

        public async Task LoadAsync()
        {
            IsBusy = true;


            Angels = await _angelService.GetAngelsAsync();
            SelectedAngel = null;
            //SelectedSponsor =  await _sponsorService.GetSponsorAsync(Preferences.Get("selected_sponsor", -1));
            //Preferences.Clear();

            IsBusy = false;
        }

        public void Subscribe()
        {
            _messageService.Subscribe<AngelListViewModel, Angel>(this, OnAngelSaved);
        }

        public void Unsubscribe()
        {
            _messageService.Unsubscribe(this);
        }

        private async void OnAngelSaved(AngelListViewModel sender, string message, Angel changed)
        {
            if (changed != null)
            {
                await _contextService.RunAsync(async () =>
                {
                    var savedAngel = await _angelService.GetAngelAsync(changed.Id);
                    //int angelIndex = Angels.IndexOf(Angels.Where(a => a.Id == savedAngel.Id).FirstOrDefault());
                    var listAngelIndex = Angels.IndexOf(Angels.Where(a => a.Id == changed.Id).FirstOrDefault());

                    switch (message)
                    {
                        case "NewAngelSaved":
                            Angels.Add(savedAngel);
                            break;
                        case "AngelChanged":
                            Angels[listAngelIndex] = savedAngel;
                            break;
                        case "AngelDeleted":
                            Angels.RemoveAt(listAngelIndex);
                            break;
                    }
                });
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
    }
}
