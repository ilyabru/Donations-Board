using DonationBoard.Configuration;
using DonationBoard.Models;
using DonationBoard.Services;
using DonationBoard.ViewModels.Base;
using DonationBoard.Common;
using GearVrController4Windows;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;

namespace DonationBoard.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private IDonorService _donorService;
        private IContextService _contextService;
        private INavigationService _navigationService;
        private IMessageService _messageService;

        private ObservableCollection<Donor> donors;
        private Donor selectedDonor;
        private bool isViewing = false;

        public MainPageViewModel(IDonorService donorService,
                                 IContextService contextService,
                                 INavigationService navigationService,
                                 IMessageService messageService)
        {
            GVRC = ServiceLocator.Current.GetService<GearVrController>();

            _donorService = donorService;
            _contextService = contextService;
            _navigationService = navigationService;
            _messageService = messageService;
        }

        public GearVrController GVRC { get; set; }

        public ObservableCollection<Donor> Donors
        {
            get => donors;
            set
            {
                SetPropertyValue(ref donors, value);
                RaisePropertyChanged(nameof(TotalAmount));
            }
        }

        public Donor SelectedDonor
        {
            get => selectedDonor;
            set
            {
                // sets isViewed property when navigating the flipview
                if (selectedDonor != null
                    && selectedDonor != value
                    && value != null
                    && isViewing == true
                    && SelectedDonor.IsViewed == false)
                {
                    SelectedDonor.IsViewed = true;
                    _donorService.UpdateDonorAsync(SelectedDonor);
                    _messageService.Send(this, "DonorViewed", SelectedDonor);
                }

                SetPropertyValue(ref selectedDonor, value);
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
                    && SelectedDonor != null
                    && SelectedDonor.IsViewed == false)
                {
                    SelectedDonor.IsViewed = true;
                    _donorService.UpdateDonorAsync(SelectedDonor);
                    _messageService.Send(this, "DonorViewed", SelectedDonor);
                }

                SetPropertyValue(ref isViewing, value);
            }
        }

        public decimal? TotalAmount => Donors?.Sum(a => a.Amount);

        public ICommand EditDonors => new RelayCommand(async () => await OnEditDonors());

        public async Task LoadAsync()
        {
            IsBusy = true;

            // navigates to control panel on startup
            await _navigationService.CreateNewViewAsync<ControlPanelViewModel>(Donors);

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

            Donors = null;
            SelectedDonor = null;

            try
            {
                Donors = await _donorService.GetDonorsAsync();
                SelectedDonor = null;
            }
            catch
            {
                Donors = new ObservableCollection<Donor>();
                // log error

                isOk = false;
                throw;
            }

            //SelectedAngel = Angels.FirstOrDefault();

            return isOk;
        }

        public void Subscribe()
        {
            _messageService.Subscribe<DonorListViewModel, Donor>(this, OnDonorSaved);
            _messageService.Subscribe<SettingsViewModel, Guid>(this, OnSessionChanged);
        }

        public void Unsubscribe()
        {
            _messageService.Unsubscribe(this);
        }

        private async void OnDonorSaved(DonorListViewModel sender, string message, Donor changed)
        {
            if (changed != null)
            {
                await _contextService.RunAsync(async () =>
                {
                    var savedDonor = await _donorService.GetDonorAsync(changed.Id);
                    var listDonorIndex = Donors.IndexOf(Donors.FirstOrDefault(a => a.Id == changed.Id));

                    switch (message)
                    {
                        case "NewDonorSaved":
                            Donors.Add(savedDonor);
                            break;
                        case "DonorChanged":
                            Donors[listDonorIndex].Merge(savedDonor);
                            Donors[listDonorIndex].NotifyChanges();
                            break;
                        case "DonorDeleted":
                            Donors.RemoveAt(listDonorIndex);
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

        private void AddEditDonor(Donor donor)
        {
            int index = Donors.IndexOf(Donors.Where(a => a.Id == donor.Id).First());
            Donors[index] = donor;
        }

        private async Task OnEditDonors()
        {
            await _navigationService.CreateNewViewAsync<ControlPanelViewModel>(Donors);
        }
    }
}
