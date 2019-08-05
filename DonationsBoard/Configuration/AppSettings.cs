using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DonationBoard.Configuration
{
    public class AppSettings
    {
        static AppSettings()
        {
            Current = new AppSettings();
        }

        public static AppSettings Current { get; }

        public ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;

        public Guid CurrentSession
        {
            get => GetSettingsValue("CurrentSession", default(Guid));
            set => LocalSettings.Values["CurrentSession"] = (Guid)value;
        }

        // ID of the BLE controller used on previous run
        public string CurrentController
        {
            get => GetSettingsValue("CurrentController", default(string));
            set => LocalSettings.Values["CurrentController"] = value;
        }

        private TResult GetSettingsValue<TResult>(string name, TResult defaultValue)
        {
            try
            {
                if (!LocalSettings.Values.ContainsKey(name))
                {
                    LocalSettings.Values[name] = defaultValue;
                }
                return (TResult)LocalSettings.Values[name];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return defaultValue;
            }
        }
    }
}
