using AngelBoard.ViewModels.Base;
using AngelBoard.Views;
using GalaSoft.MvvmLight.Command;
using GearVrController4Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AngelBoard.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly GearVrController _gearVrController;

        private DeviceInformationDisplay selectedDevice;

        public SettingsViewModel()
        {
            _gearVrController = ServiceLocator.Current.GetService<GearVrController>();
            _gearVrController.PropertyChanged += _gearVrControllerService_PropertyChanged;
        }

        public DeviceInformationDisplay SelectedDevice
        {
            get => selectedDevice;
            set => SetPropertyValue(ref selectedDevice, value);
        }

        public ICommand DeviceSelected => new RelayCommand(OnDeviceSelected);

        private void OnDeviceSelected()
        {
            _gearVrController.Create(SelectedDevice.DeviceInformation);
        }

        private void _gearVrControllerService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var x = _gearVrController.TouchpadButton;
        }
    }
}
