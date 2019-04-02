using AngelBoard.Common;
using AngelBoard.Configuration;
using AngelBoard.Models;
using AngelBoard.Services;
using AngelBoard.ViewModels.Base;
using DonationsBoard.Common;
using GearVrController4Windows;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Windows.Foundation;

namespace AngelBoard.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly ISessionService _sessionService;
        private readonly IMessageService _messageService;
        private readonly IContextService _contextService;

        private readonly GearVrController _gearVrController;

        private ObservableCollection<Session> sessions;
        private Session selectedSession;
        private Session currentSession;

        private DeviceWatcher deviceWatcher = null;
        private TypedEventHandler<DeviceWatcher, DeviceInformation> handlerAdded = null;
        private TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> handlerUpdated = null;
        private TypedEventHandler<DeviceWatcher, DeviceInformationUpdate> handlerRemoved = null;
        private TypedEventHandler<DeviceWatcher, object> handlerEnumCompleted = null;
        private TypedEventHandler<DeviceWatcher, object> handlerStopped = null;

        private ObservableCollection<DeviceInformationDisplay> bLEDeviceResultCollection;
        private bool isPairing;
        private DeviceInformationDisplay selectedDevice;

        public SettingsViewModel(ISessionService sessionService,
                                 IMessageService messageService,
                                 IContextService contextService)
        {
            _sessionService = sessionService;
            _messageService = messageService;
            _contextService = contextService;

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

        // Contains collection of discovered Bluetooth Low Energy 
        public ObservableCollection<DeviceInformationDisplay> BLEDeviceResultCollection
        {
            get => bLEDeviceResultCollection;
            set => SetPropertyValue(ref bLEDeviceResultCollection, value);
        }

        // This value determines if BLE related controls are enabled
        public bool IsPairing
        {
            get => isPairing;
            set => SetPropertyValue(ref isPairing, value);
        }

        public DeviceInformationDisplay SelectedDevice
        {
            get => selectedDevice;
            set
            {
                SetPropertyValue(ref selectedDevice, value);
                RaisePropertyChanged(nameof(IsDeviceSelected));
            }
        }

        public bool IsDeviceSelected
        {
            get { return SelectedDevice != null; }
        }

        public ICommand PairDevice => new RelayCommand(async () => await OnPairDevice());
        public ICommand UnpairDevice => new RelayCommand(async () => await OnUnpairDevice());
        public ICommand DeviceSelected => new RelayCommand(OnDeviceSelected);

        public ICommand SessionLoad => new RelayCommand(async () => await OnSessionLoad());
        public ICommand SessionCreate => new RelayCommand(async () => await OnSessionCreated());

        private async Task OnPairDevice()
        {
            IsPairing = true;

            DevicePairingResult dpr = await SelectedDevice.DeviceInformation.Pairing.PairAsync();

            // TODO notify of result

            IsPairing = false;
        }

        private async Task OnUnpairDevice()
        {
            IsPairing = true;

            DeviceUnpairingResult dupr = await SelectedDevice.DeviceInformation.Pairing.UnpairAsync();

            // todo notfify of result

            IsPairing = false;
        }

        private void OnDeviceSelected()
        {
            _gearVrController.Create(SelectedDevice.DeviceInformation);

            AppSettings.Current.CurrentController = SelectedDevice.DeviceInformation.Id;
        }

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

        public void StartWatcher()
        {
            //startWatcherButton.IsEnabled = false;
            BLEDeviceResultCollection.Clear();

            // watch for Bluetooth LE devices
            deviceWatcher = DeviceInformation.CreateWatcher(
                "System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\"",
                null, // don't request additional properties
                DeviceInformationKind.AssociationEndpoint);

            // Hook up handlers for the watcher events before starting the watcher

            handlerAdded = new TypedEventHandler<DeviceWatcher, DeviceInformation>(async (watcher, deviceInfo) =>
            {
                await _contextService.RunAsync(() =>
                {
                    BLEDeviceResultCollection.Add(new DeviceInformationDisplay(deviceInfo));

                    // notify user
                });
            });
            deviceWatcher.Added += handlerAdded;

            handlerUpdated = new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(async (watcher, deviceInfoUpdate) =>
            {
                // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
                await _contextService.RunAsync(() =>
                {
                    // Find the corresponding updated DeviceInformation in the collection and pass the update object
                    // to the Update method of the existing DeviceInformation. This automatically updates the object
                    // for us.
                    foreach (DeviceInformationDisplay deviceInfoDisp in BLEDeviceResultCollection)
                    {
                        if (deviceInfoDisp.Id == deviceInfoUpdate.Id)
                        {
                            deviceInfoDisp.Update(deviceInfoUpdate);
                            break;
                        }
                    }
                });
            });
            deviceWatcher.Updated += handlerUpdated;

            handlerRemoved = new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(async (watcher, deviceInfoUpdate) =>
            {
                // Since we have the collection databound to a UI element, we need to update the collection on the UI thread.
                await _contextService.RunAsync(() =>
                {
                    // Find the corresponding DeviceInformation in the collection and remove it
                    foreach (DeviceInformationDisplay deviceInfoDisp in BLEDeviceResultCollection)
                    {
                        if (deviceInfoDisp.Id == deviceInfoUpdate.Id)
                        {
                            BLEDeviceResultCollection.Remove(deviceInfoDisp);
                            break;
                        }
                    }

                    //rootPage.NotifyUser(
                    //    String.Format("{0} devices found.", BLEDeviceResultCollection.Count),
                    //    NotifyType.StatusMessage);
                });
            });
            deviceWatcher.Removed += handlerRemoved;

            handlerEnumCompleted = new TypedEventHandler<DeviceWatcher, Object>(async (watcher, obj) =>
            {
                await _contextService.RunAsync(() =>
                {
                    //rootPage.NotifyUser(
                    //    String.Format("{0} devices found. Enumeration completed. Watching for updates...", BLEDeviceResultCollection.Count),
                    //    NotifyType.StatusMessage);
                });
            });
            deviceWatcher.EnumerationCompleted += handlerEnumCompleted;

            handlerStopped = new TypedEventHandler<DeviceWatcher, Object>(async (watcher, obj) =>
            {
                await _contextService.RunAsync(() =>
                {
                    //rootPage.NotifyUser(
                    //    String.Format("{0} devices found. Watcher {1}.",
                    //        BLEDeviceResultCollection.Count,
                    //        DeviceWatcherStatus.Aborted == watcher.Status ? "aborted" : "stopped"),
                    //    NotifyType.StatusMessage);
                });
            });
            deviceWatcher.Stopped += handlerStopped;

            deviceWatcher.Start();
            // stopWatcherButton.IsEnabled = true;
        }

        public void StopWatcher()
        {
            // stopWatcherButton.IsEnabled = false;

            if (deviceWatcher != null)
            {
                // First unhook all event handlers except the stopped handler. This ensures our
                // event handlers don't get called after stop, as stop won't block for any "in flight" 
                // event handler calls.  We leave the stopped handler as it's guaranteed to only be called
                // once and we'll use it to know when the query is completely stopped. 
                deviceWatcher.Added -= handlerAdded;
                deviceWatcher.Updated -= handlerUpdated;
                deviceWatcher.Removed -= handlerRemoved;
                deviceWatcher.EnumerationCompleted -= handlerEnumCompleted;

                if (deviceWatcher.Status == DeviceWatcherStatus.Started ||
                    deviceWatcher.Status == DeviceWatcherStatus.EnumerationCompleted)
                {
                    deviceWatcher.Stop();
                }
            }
            // startWatcherButton.IsEnabled = true;
        }

        public async Task LoadAsync()
        {
            IsBusy = true;

            BLEDeviceResultCollection = new ObservableCollection<DeviceInformationDisplay>();
            StartWatcher();

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
