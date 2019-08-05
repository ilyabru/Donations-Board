using System;
using System.Collections.Concurrent;
using DonationBoard.Services;
using DonationBoard.ViewModels;
using GearVrController4Windows;
using Microsoft.Extensions.DependencyInjection;
using Windows.UI.ViewManagement;

namespace DonationBoard
{
    public class ServiceLocator : IDisposable
    {
        static private readonly ConcurrentDictionary<int, ServiceLocator> _serviceLocators = new ConcurrentDictionary<int, ServiceLocator>();

        static private ServiceProvider _rootServiceProvider = null;

        static public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<ISQLiteService, MockSQLiteService>();
            serviceCollection.AddSingleton<ISessionService, MockSessionService>();
            serviceCollection.AddSingleton<IDonorService, MockDonorService>();
            serviceCollection.AddSingleton<IStatsService, MockStatsService>();

            serviceCollection.AddSingleton<IMessageService, MessageService>();
            serviceCollection.AddSingleton<IDialogService, DialogService>();
            serviceCollection.AddSingleton<GearVrController>();

            serviceCollection.AddScoped<IContextService, ContextService>();
            serviceCollection.AddScoped<INavigationService, NavigationService>();

            // view models
            serviceCollection.AddTransient<MainPageViewModel>();
            serviceCollection.AddTransient<ControlPanelViewModel>();
            serviceCollection.AddTransient<DonorListViewModel>();
            serviceCollection.AddTransient<StatsViewModel>();
            serviceCollection.AddTransient<SettingsViewModel>();

            _rootServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public static ServiceLocator Current
        {
            get
            {
                int currentViewId = ApplicationView.GetForCurrentView().Id;
                return _serviceLocators.GetOrAdd(currentViewId, key => new ServiceLocator());
            }
        }

        public static void DisposeCurrent()
        {
            int currentViewId = ApplicationView.GetForCurrentView().Id;
            if (_serviceLocators.TryRemove(currentViewId, out ServiceLocator current))
            {
                current.Dispose();
            }
        }

        private IServiceScope _serviceScope = null;

        public ServiceLocator()
        {
            _serviceScope = _rootServiceProvider.CreateScope();
        }

        public T GetService<T>()
        {
            return GetService<T>(true);
        }

        public T GetService<T>(bool isRequired)
        {
            if (isRequired)
            {
                return _serviceScope.ServiceProvider.GetRequiredService<T>();
            }
            return _serviceScope.ServiceProvider.GetService<T>();
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_serviceScope != null)
                {
                    _serviceScope.Dispose();
                }
            }
        }
        #endregion
    }
}
