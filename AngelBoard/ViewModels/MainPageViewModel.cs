using AngelBoard.Configuration;
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
                    _messageService.Send(this, "AngelViewed", SelectedAngel);
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
                    _messageService.Send(this, "AngelViewed", SelectedAngel);
                }

                SetPropertyValue(ref isViewing, value);
            }
        }

        public ICommand EditAngels => new RelayCommand(async () => await OnEditAngels());

        public async Task LoadAsync()
        {
            IsBusy = true;

            await RefreshAsync();
            SelectedAngel = null;

            // navigates to control panel on startup
            await _navigationService.CreateNewViewAsync<ControlPanelViewModel>(Angels);

            IsBusy = false;
        }

        public async Task<bool> RefreshAsync()
        {
            bool isOk = true;

            try
            {
                Angels = await _angelService.GetAngelsAsync();
            }
            catch (Exception ex)
            {
                Angels = new ObservableCollection<Angel>();
                // log error

                isOk = false;
                throw;
            }

            return isOk;
        }

        public void Subscribe()
        {
            _messageService.Subscribe<AngelListViewModel, Angel>(this, OnAngelSaved);
            _messageService.Subscribe<SettingsViewModel, Guid>(this, OnSessionChanged);
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
                    var listAngelIndex = Angels.IndexOf(Angels.FirstOrDefault(a => a.Id == changed.Id));

                    switch (message)
                    {
                        case "NewAngelSaved":
                            Angels.Add(savedAngel);
                            break;
                        case "AngelChanged":
                            Angels[listAngelIndex].Merge(savedAngel);
                            Angels[listAngelIndex].NotifyChanges();
                            break;
                        case "AngelDeleted":
                            Angels.RemoveAt(listAngelIndex);
                            break;
                    }
                });
            }
        }

        private async void OnSessionChanged(SettingsViewModel sender, string message, Guid changedSessionId)
        {
            await _contextService.RunAsync(async () =>
            {
                if (message == "SessionChanged")
                {
                    // make sure appsettings was updated even though it should be
                    if (changedSessionId != AppSettings.Current.CurrentSession)
                        AppSettings.Current.CurrentSession = changedSessionId;

                    await RefreshAsync();
                }
            });
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
    }
}
