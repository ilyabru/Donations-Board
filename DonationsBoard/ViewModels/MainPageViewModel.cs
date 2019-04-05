using AngelBoard.Configuration;
using AngelBoard.Models;
using AngelBoard.Services;
using AngelBoard.ViewModels.Base;
using DonationsBoard.Common;
using GearVrController4Windows;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;

namespace AngelBoard.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private IAngelService _angelService;
        private IContextService _contextService;
        private INavigationService _navigationService;
        private IMessageService _messageService;

        private ObservableCollection<Donor> angels;
        private Donor selectedAngel;
        private bool isViewing = false;

        public MainPageViewModel(IAngelService angelService,
                                 IContextService contextService,
                                 INavigationService navigationService,
                                 IMessageService messageService)
        {
            GVRC = ServiceLocator.Current.GetService<GearVrController>();

            _angelService = angelService;
            _contextService = contextService;
            _navigationService = navigationService;
            _messageService = messageService;
        }

        public GearVrController GVRC { get; set; }

        public ObservableCollection<Donor> Angels
        {
            get => angels;
            set
            {
                SetPropertyValue(ref angels, value);
                RaisePropertyChanged(nameof(TotalAmount));
            }
        }

        public Donor SelectedAngel
        {
            get => selectedAngel;
            set
            {
                // sets isViewed property when navigating the flipview
                if (selectedAngel != null
                    && selectedAngel != value
                    && value != null
                    && isViewing == true
                    && SelectedAngel.IsViewed == false)
                {
                    SelectedAngel.IsViewed = true;
                    _angelService.UpdateAngelAsync(SelectedAngel);
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
                if (isViewing == true
                    && value == false
                    && SelectedAngel != null
                    && SelectedAngel.IsViewed == false)
                {
                    SelectedAngel.IsViewed = true;
                    _angelService.UpdateAngelAsync(SelectedAngel);
                    _messageService.Send(this, "AngelViewed", SelectedAngel);
                }

                SetPropertyValue(ref isViewing, value);
            }
        }

        public decimal? TotalAmount => Angels?.Sum(a => a.Amount);

        public ICommand EditAngels => new RelayCommand(async () => await OnEditAngels());

        public async Task LoadAsync()
        {
            IsBusy = true;

            // navigates to control panel on startup
            await _navigationService.CreateNewViewAsync<ControlPanelViewModel>(Angels);

            // populate angel list
            await RefreshAsync();

            // try to retreive previously used controller
            var ccId = AppSettings.Current.CurrentController;
            if (!string.IsNullOrEmpty(ccId))
            {
                try
                {
                    var savedDeviceInfo = await DeviceInformation.CreateFromIdAsync(ccId); // TODO: add check that this is proper device ID
                    await GVRC.Create(savedDeviceInfo);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            IsBusy = false;
        }

        public async Task<bool> RefreshAsync()
        {
            bool isOk = true;

            Angels = null;
            SelectedAngel = null;

            try
            {
                Angels = await _angelService.GetAngelsAsync();
                SelectedAngel = null;
            }
            catch (Exception ex)
            {
                Angels = new ObservableCollection<Donor>();
                // log error

                isOk = false;
                throw;
            }

            //SelectedAngel = Angels.FirstOrDefault();

            return isOk;
        }

        public void Subscribe()
        {
            _messageService.Subscribe<AngelListViewModel, Donor>(this, OnAngelSaved);
            _messageService.Subscribe<SettingsViewModel, Guid>(this, OnSessionChanged);
        }

        public void Unsubscribe()
        {
            _messageService.Unsubscribe(this);
        }

        private async void OnAngelSaved(AngelListViewModel sender, string message, Donor changed)
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
                    RaisePropertyChanged(nameof(TotalAmount));
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

        private void AddEditAngel(Donor angel)
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
