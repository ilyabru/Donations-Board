using AngelBoard.Configuration;
using AngelBoard.Models;
using AngelBoard.Services;
using AngelBoard.ViewModels.Base;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;

namespace AngelBoard.ViewModels
{
    public class AngelListViewModel : BaseViewModel
    {
        private readonly IAngelService _angelService;
        private readonly IMessageService _messageService;
        private readonly IDialogService _dialogService;
        private readonly IContextService _contextService;

        private ObservableCollection<Angel> angels;
        private Angel selectedAngel;
        private Angel inputAngel;
        private bool isNew = true;

        private ObservableCollection<string> locations;

        public AngelListViewModel(IAngelService angelService,
                                  IMessageService messageService,
                                  IDialogService dialogService,
                                  IContextService contextService)
        {
            _angelService = angelService;
            _messageService = messageService;
            _dialogService = dialogService;
            _contextService = contextService;
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

        public ObservableCollection<string> Locations
        {
            get => locations;
            set { SetPropertyValue(ref locations, value); }
        }

        public ICommand SaveAngel => new RelayCommand(async () => await OnSaveAngel());
        public ICommand EditAngel => new RelayCommand(OnEditAngel);
        public ICommand CancelEditAngel => new RelayCommand(OnCancelEditAngel);
        public ICommand DeleteAngel => new RelayCommand(async () => await OnDeleteAngel());

        public async Task LoadAsync()
        {
            IsBusy = true;

            await RefreshAsync();
            InputAngel = new Angel();

            IsBusy = false;
        }

        public async Task RefreshAsync()
        {
            Angels = await _angelService.GetAngelsAsync();
            await RefreshLocations();
        }

        public async Task RefreshLocations()
        {
            Locations = await _angelService.GetLocations();
        }

        public void Subscribe()
        {
            _messageService.Subscribe<MainPageViewModel, Angel>(this, OnAngelUpdated);
            _messageService.Subscribe<SettingsViewModel, Guid>(this, OnSessionChanged);
        }

        public void Unsubscribe()
        {
            _messageService.Unsubscribe(this);
        }

        private async void OnAngelUpdated(MainPageViewModel sender, string message, Angel changed)
        {
            if (changed != null)
            {
                await _contextService.RunAsync(async () =>
                {
                    var savedAngel = await _angelService.GetAngelAsync(changed.Id);
                    var viewedAngel = Angels.FirstOrDefault(a => a.Id == changed.Id);

                    switch (message)
                    {
                        case "AngelViewed":
                            savedAngel.IsViewed = true;
                            await _angelService.UpdateAngelAsync(savedAngel);
                            viewedAngel.IsViewed = true;
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

        protected IEnumerable<IValidationConstraint<Angel>> GetValidationConstraints(Angel model)
        {
            yield return new RequiredConstraint<Angel>("Name", model.Name);
            yield return new RequiredConstraint<Angel>("Location", model.Location);
            yield return new RequiredGreaterThanZeroConstraint<Angel>("Amount", model.Amount);
        }

        private Result Validate(Angel model)
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

        private async Task OnSaveAngel()
        {
            // SelectedAngel.Merge(InputAngel);
            var result = Validate(InputAngel);
            if (result.IsOk)
            {
                if (IsNew)
                {
                    await _angelService.AddAngelAsync(InputAngel);
                    Angels.Add(InputAngel);

                    _messageService.Send(this, "NewAngelSaved", InputAngel);
                }
                else
                {
                    await _angelService.UpdateAngelAsync(InputAngel);
                    SelectedAngel.Merge(InputAngel);

                    SelectedAngel = null; // reset listview

                    _messageService.Send(this, "AngelChanged", InputAngel);
                }

                // Clear textboxes
                IsNew = true;
                InputAngel = new Angel();
                await RefreshLocations();
            }
            else
            {
                await _dialogService.ShowAsync(result.Message, $"{result.Description} Please, correct the error and try again.");
            }
        }

        private void OnEditAngel()
        {
            IsNew = false;

            InputAngel.Merge(SelectedAngel);
        }

        private void OnCancelEditAngel()
        {
            IsNew = true;
            InputAngel = new Angel();
        }

        private async Task OnDeleteAngel()
        {
            await _angelService.DeleteAngelAsync(SelectedAngel);
            _messageService.Send(this, "AngelDeleted", SelectedAngel);
            Angels.Remove(SelectedAngel);
            await RefreshLocations();
        }
    }
}
