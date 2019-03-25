using System;
using System.Collections.Generic;
using Windows.Devices.Enumeration;
using Windows.UI.Xaml.Media.Imaging;

namespace AngelBoard.Common
{
    public class DeviceInformationDisplay : ObservableObject
    {
        private DeviceInformation deviceInfo;

        public DeviceInformationDisplay(DeviceInformation deviceInfoIn)
        {
            deviceInfo = deviceInfoIn;
            UpdateGlyphBitmapImage();
        }

        public DeviceInformationKind Kind => deviceInfo.Kind;

        public string Id => deviceInfo.Id;

        public string Name => deviceInfo.Name;

        public BitmapImage GlyphBitmapImage
        {
            get;
            private set;
        }

        public bool CanPair => deviceInfo.Pairing.CanPair && !deviceInfo.Pairing.IsPaired;

        public bool IsPaired => deviceInfo.Pairing.IsPaired;

        public IReadOnlyDictionary<string, object> Properties => deviceInfo.Properties;

        public DeviceInformation DeviceInformation
        {
            get => deviceInfo;
            private set => deviceInfo = value;
        }

        public void Update(DeviceInformationUpdate deviceInfoUpdate)
        {
            deviceInfo.Update(deviceInfoUpdate);

            RaisePropertyChanged("Kind");
            RaisePropertyChanged("Id");
            RaisePropertyChanged("Name");
            RaisePropertyChanged("DeviceInformation");
            RaisePropertyChanged("CanPair");
            RaisePropertyChanged("IsPaired");

            UpdateGlyphBitmapImage();
        }

        private async void UpdateGlyphBitmapImage()
        {
            DeviceThumbnail deviceThumbnail = await deviceInfo.GetGlyphThumbnailAsync();
            BitmapImage glyphBitmapImage = new BitmapImage();
            await glyphBitmapImage.SetSourceAsync(deviceThumbnail);
            GlyphBitmapImage = glyphBitmapImage;
            RaisePropertyChanged("GlyphBitmapImage");
        }
    }
}
