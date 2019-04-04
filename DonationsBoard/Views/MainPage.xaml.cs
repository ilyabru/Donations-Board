using AngelBoard.Services;
using AngelBoard.ViewModels;
using System.ComponentModel;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace AngelBoard.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            ViewModel = ServiceLocator.Current.GetService<MainPageViewModel>();

            InitializeComponent();
            InitlializeContext();
            InitializeNavigation();

            BackgroundMedia.MediaPlayer.IsLoopingEnabled = true;
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

        private void Gvc_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ViewModel.GVRC.TouchpadButton)
            {
                var OldestNonViewedAngel = ViewModel.Angels.Where(a => a.IsViewed == false).OrderBy(a => a.CreatedDate).FirstOrDefault();
                var SecondOldestNonViewedAngel = ViewModel.Angels.Where(a => a.IsViewed == false).OrderBy(a => a.CreatedDate).Skip(1).FirstOrDefault();

                if (!AngelPopup.IsOpen && OldestNonViewedAngel != null) // popup opened and oldest non viewed donor is shown
                {
                    fvAngels.SelectedItem = OldestNonViewedAngel;
                    AngelPopup.IsOpen = true;
                }
                else if (fvAngels.SelectedItem == OldestNonViewedAngel && SecondOldestNonViewedAngel != null) // move to next non viewed donor if exists
                {
                    fvAngels.SelectedItem = SecondOldestNonViewedAngel;
                }
                else // all donors viewed, therefore close
                {
                    AngelPopup.IsOpen = false;
                }
            }

            if (ViewModel.GVRC.BackButton)
            {
                AngelPopup.IsOpen = false;
            }
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
            gvAngels.ItemHeight = e.NewSize.Height / 6;
        }

        private void ControlPanelInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            args.Handled = true;
        }

        private void FullscreenInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
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

        private void EscapeInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            AngelPopup.IsOpen = false;
        }
    }
}
