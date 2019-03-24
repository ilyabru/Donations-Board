using AngelBoard.Configuration;
using AngelBoard.Models;
using AngelBoard.Services;
using AngelBoard.ViewModels.Base;
using AngelBoard.Views;
using GalaSoft.MvvmLight.Command;
using GearVrController4Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AngelBoard.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISessionService _sessionService;
        private readonly IMessageService _messageService;
        private readonly GearVrController _gearVrController;

        private ObservableCollection<Session> sessions;
        private Session selectedSession;
        private Session currentSession;

        private DeviceInformationDisplay selectedDevice;

        public SettingsViewModel(ISessionService sessionService,
                                 IMessageService messageService)
        {
            _sessionService = sessionService;
            _messageService = messageService;

            _gearVrController = ServiceLocator.Current.GetService<GearVrController>();
        }

        public ObservableCollection<Session> Sessions
        {
            get => sessions;
            set => SetPropertyValue(ref sessions, value);
        }

        public Session SelectedSession
        {
            get => selectedSession;
            set
            {
                SetPropertyValue(ref selectedSession, value);
                RaisePropertyChanged(nameof(IsSessionSelected));
            }
        }

        public Session CurrentSession
        {
            get => currentSession;
            set => SetPropertyValue(ref currentSession, value);
        }

        public bool IsSessionSelected
        {
            get { return SelectedSession != null; }
        }

        public DeviceInformationDisplay SelectedDevice
        {
            get => selectedDevice;
            set => SetPropertyValue(ref selectedDevice, value);
        }

        public ICommand DeviceSelected => new RelayCommand(OnDeviceSelected);
        public ICommand SessionLoad => new RelayCommand(async () => await OnSessionLoad());
        public ICommand SessionCreate => new RelayCommand(async () => await OnSessionCreated());

        private async Task OnSessionLoad()
        {
            // add new session id to appsettings
            AppSettings.Current.CurrentSession = SelectedSession.Id;

            // send message to refresh main page and control panel
            _messageService.Send(this, "SessionChanged", AppSettings.Current.CurrentSession);
            await RefreshAsync();
        }

        private async Task OnSessionCreated()
        {
            Guid newSessionId = await _sessionService.CreateSession();
            // add new session id to appsettings
            AppSettings.Current.CurrentSession = newSessionId;

            // send message to refresh main page and control panel
            _messageService.Send(this, "SessionChanged", AppSettings.Current.CurrentSession); // TODO  make the parameter optional?
            await RefreshAsync();
        }

        private void OnDeviceSelected()
        {
            _gearVrController.Create(SelectedDevice.DeviceInformation);
        }

        public async Task LoadAsync()
        {
            IsBusy = true;

            await RefreshAsync();

            IsBusy = false;
        }

        public async Task RefreshAsync()
        {
            Sessions = await _sessionService.GetSessions();
            CurrentSession = await _sessionService.GetSession(AppSettings.Current.CurrentSession);
        }
    }
}
