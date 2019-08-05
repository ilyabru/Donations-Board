using DonationBoard.Configuration;
using DonationBoard.Models;
using DonationBoard.Services;
using DonationBoard.ViewModels.Base;
using DonationBoard.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DonationBoard.ViewModels
{
    public class DonorListViewModel : BaseViewModel
    {
        private readonly IDonorService _donorService;
        private readonly IMessageService _messageService;
        private readonly IDialogService _dialogService;
        private readonly IContextService _contextService;

        private ObservableCollection<Donor> donors;
        private Donor selectedDonor;
        private Donor inputDonor;
        private bool isNew = true;

        private ObservableCollection<string> locations;

        public DonorListViewModel(IDonorService donorService,
                                  IMessageService messageService,
                                  IDialogService dialogService,
                                  IContextService contextService)
        {
            _donorService = donorService;
            _messageService = messageService;
            _dialogService = dialogService;
            _contextService = contextService;
        }

        public ObservableCollection<Donor> Donors
        {
            get { return donors; }
            set { SetPropertyValue(ref donors, value); }
        }
        public Donor SelectedDonor
        {
            get { return selectedDonor; }
            set
            {
                SetPropertyValue(ref selectedDonor, value);
                RaisePropertyChanged(nameof(IsDonorSelected));
            }
        }

        public bool IsDonorSelected
        {
            get { return SelectedDonor != null; }
        }

        public Donor InputDonor
        {
            get { return inputDonor; }
            set { SetPropertyValue(ref inputDonor, value); }
        }

        public bool IsNew
        {
            get => isNew;
            set { SetPropertyValue(ref isNew, value); }
        }

        public ObservableCollection<string> Locations
        {
            get => locations;
            set { SetPropertyValue(ref locations, value); }
        }

        public ICommand SaveDonor => new RelayCommand(async () => await OnSaveDonor());
        public ICommand EditDonor => new RelayCommand(OnEditDonor);
        public ICommand CancelEditDonor => new RelayCommand(OnCancelEditDonor);
        public ICommand DeleteDonor => new RelayCommand(async () => await OnDeleteDonor());

        public async Task LoadAsync()
        {
            IsBusy = true;

            await RefreshAsync();
            InputDonor = new Donor();

            IsBusy = false;
        }

        public async Task RefreshAsync()
        {
            Donors = await _donorService.GetDonorsAsync();
            await RefreshLocations();
        }

        public async Task RefreshLocations()
        {
            Locations = await _donorService.GetLocations();
        }

        public void Subscribe()
        {
            _messageService.Subscribe<MainPageViewModel, Donor>(this, OnDonorUpdated);
            _messageService.Subscribe<SettingsViewModel, Guid>(this, OnSessionChanged);
        }

        public void Unsubscribe()
        {
            _messageService.Unsubscribe(this);
        }

        private async void OnDonorUpdated(MainPageViewModel sender, string message, Donor changed)
        {
            if (changed != null)
            {
                await _contextService.RunAsync(async () =>
                {
                    var savedDonor = await _donorService.GetDonorAsync(changed.Id);
                    var viewedDonor = Donors.FirstOrDefault(a => a.Id == changed.Id);

                    switch (message)
                    {
                        case "DonorViewed":
                            savedDonor.IsViewed = true;
                            //await _angelService.UpdateAngelAsync(savedAngel);
                            viewedDonor.IsViewed = true;
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

        protected IEnumerable<IValidationConstraint<Donor>> GetValidationConstraints(Donor model)
        {
            yield return new RequiredConstraint<Donor>("Name", model.Name);
            yield return new RequiredConstraint<Donor>("Location", model.Location);
            yield return new RequiredGreaterThanZeroConstraint<Donor>("Amount", model.Amount);
        }

        private Result Validate(Donor model)
        {
            foreach (var constraint in GetValidationConstraints(model))
            {
                if (!constraint.Validate())
                {
                    return Result.Error("Validation Error", constraint.Message);
                }
            }
            return Result.Ok();
        }

        private async Task OnSaveDonor()
        {
            // SelectedAngel.Merge(InputAngel);
            var result = Validate(InputDonor);
            if (result.IsOk)
            {
                if (IsNew)
                {
                    await _donorService.AddDonorAsync(InputDonor);

                    _messageService.Send(this, "NewDonorSaved", InputDonor);
                }
                else
                {
                    await _donorService.UpdateDonorAsync(InputDonor);
                    SelectedDonor.Merge(InputDonor);

                    SelectedDonor = null; // reset listview

                    _messageService.Send(this, "DonorChanged", InputDonor);
                }

                // Clear textboxes
                IsNew = true;
                InputDonor = new Donor();
                await RefreshAsync();
            }
            else
            {
                await _dialogService.ShowAsync(result.Message, $"{result.Description} Please, correct the error and try again.");
            }
        }

        private void OnEditDonor()
        {
            IsNew = false;

            InputDonor.Merge(SelectedDonor);
        }

        private void OnCancelEditDonor()
        {
            IsNew = true;
            InputDonor = new Donor();
        }

        private async Task OnDeleteDonor()
        {
            if (await _dialogService.ShowAsync("Confirm delete", $@"Are you sure you want to delete ""{SelectedDonor.Name}""?", "Ok", "Cancel"))
            {
                await _donorService.DeleteDonorAsync(SelectedDonor);
                _messageService.Send(this, "DonorDeleted", SelectedDonor);
                Donors.Remove(SelectedDonor);
                await RefreshLocations();
            }
        }
    }
}
