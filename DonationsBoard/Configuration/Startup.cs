using DonationBoard.Configuration;
using DonationBoard.Services;
using DonationBoard.ViewModels;
using DonationBoard.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationBoard
{
    static public class Startup
    {
        private static readonly ServiceCollection _serviceCollection = new ServiceCollection();

        static public async Task ConfigureAsync()
        {
            ServiceLocator.Configure(_serviceCollection);

            // Initialize Session
            var sessionService = ServiceLocator.Current.GetService<ISessionService>();
            AppSettings.Current.CurrentSession = await sessionService.InitializeSession(AppSettings.Current.CurrentSession);

            ConfigureNavigation();
        }

        private static void ConfigureNavigation()
        {
            NavigationService.Register<MainPageViewModel, MainPage>();
            NavigationService.Register<ControlPanelViewModel, ControlPanelView>();
            NavigationService.Register<DonorListViewModel, DonorListView>();
            NavigationService.Register<StatsViewModel, StatsView>();
            NavigationService.Register<SettingsViewModel, SettingsView>();
        }
    }
}
