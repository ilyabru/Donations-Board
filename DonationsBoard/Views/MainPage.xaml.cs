using AngelBoard.Configuration;
using AngelBoard.Services;
using AngelBoard.ViewModels;
using GearVrController4Windows;
using System;
using System.ComponentModel;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Devices.Enumeration;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AngelBoard.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            ViewModel = ServiceLocator.Current.GetService<MainPageViewModel>();

            InitializeComponent();
            InitlializeContext();
            InitializeNavigation();

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            //Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode();

            coreTitleBar.ExtendViewIntoTitleBar = false;
        }

        private void Gvc_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ViewModel.GVRC.TouchpadButton == true)
            {
                if (!AngelPopup.IsOpen)
                {
                    fvAngels.SelectedItem = ViewModel.Angels.Where(a => a.IsViewed == false).OrderBy(a => a.Id).FirstOrDefault();
                    if (fvAngels.SelectedItem != null)
                        AngelPopup.IsOpen = true;
                }
                else
                {
                    if (fvAngels.SelectedIndex < ViewModel.Angels.Count - 1)
                    {
                        fvAngels.SetValue(FlipView.SelectedIndexProperty, fvAngels.SelectedIndex + 1);
                    }
                    else
                    {
                        AngelPopup.IsOpen = false;
                    }
                }
            }

            if (ViewModel.GVRC.BackButton == true)
            {
                AngelPopup.IsOpen = false;
            }
        }

        public MainPageViewModel ViewModel { get; set; }

        private void InitlializeContext()
        {
            var context = ServiceLocator.Current.GetService<IContextService>();
            context.Initialize(Dispatcher, ApplicationView.GetForCurrentView().Id, CoreApplication.GetCurrentView().IsMain);
        }

        private void InitializeNavigation()
        {
            var navigationService = ServiceLocator.Current.GetService<INavigationService>();
            navigationService.Initialize(frame);
            var appView = ApplicationView.GetForCurrentView();
            appView.Consolidated += OnViewConsolidated;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
            await ViewModel.LoadAsync();
            ViewModel.GVRC.PropertyChanged += Gvc_PropertyChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Unsubscribe();
        }

        // TODO: move this and other common methods to a shellview
        private void OnViewConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            ViewModel.Unsubscribe();
            ViewModel = null;
            //Bindings.StopTracking();
            var appView = ApplicationView.GetForCurrentView();
            appView.Consolidated -= OnViewConsolidated;
            ServiceLocator.DisposeCurrent();
            //Window.Current.Close();
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!AngelPopup.IsOpen)
            {
                AngelPopup.IsOpen = true;
            }
        }

        private void AngelPopup_Closed(object sender, object e)
        {
            gvAngels.SelectedItem = null;
        }

        private void GvAngels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            gvAngels.ScrollIntoView(e.AddedItems.FirstOrDefault());
        }

        // ensure only 4 rows of data exist
        private void GvAngels_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gvAngels.ItemHeight = e.NewSize.Height / 4;
        }

        private void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
        }

        private void AngelPopup_LayoutUpdated(object sender, object e)
        {
            if (fvAngels.ActualWidth == 0 && fvAngels.ActualHeight == 0)
            {
                return;
            }

            double ActualHorizontalOffset = this.AngelPopup.HorizontalOffset;
            double ActualVerticalOffset = this.AngelPopup.VerticalOffset;

            double NewHorizontalOffset = (Window.Current.Bounds.Width - fvAngels.ActualWidth) / 2;
            double NewVerticalOffset = (Window.Current.Bounds.Height - fvAngels.ActualHeight) / 2;

            if (ActualHorizontalOffset != NewHorizontalOffset || ActualVerticalOffset != NewVerticalOffset)
            {
                this.AngelPopup.HorizontalOffset = NewHorizontalOffset;
                this.AngelPopup.VerticalOffset = NewVerticalOffset;
            }
        }
    }
}
