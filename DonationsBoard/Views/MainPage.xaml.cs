using DonationBoard.Services;
using DonationBoard.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace DonationBoard.Views
{
    public sealed partial class MainPage : Page
    {
        const int maxItemsPerColumn = 4;

        public MainPage()
        {
            ViewModel = ServiceLocator.Current.GetService<MainPageViewModel>();

            InitializeComponent();
            InitlializeContext();
            InitializeNavigation();

            //BackgroundMedia.MediaPlayer.IsLoopingEnabled = true;
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
                var OldestNonViewedDonor = ViewModel.Donors.Where(a => a.IsViewed == false).OrderBy(a => a.CreatedDate).FirstOrDefault();
                var SecondOldestNonViewedDonor = ViewModel.Donors.Where(a => a.IsViewed == false).OrderBy(a => a.CreatedDate).Skip(1).FirstOrDefault();

                if (!DonorPopup.IsOpen && OldestNonViewedDonor != null) // popup opened and oldest non viewed donor is shown
                {
                    fvDonors.SelectedItem = OldestNonViewedDonor;
                    DonorPopup.IsOpen = true;
                }
                else if (fvDonors.SelectedItem == OldestNonViewedDonor && SecondOldestNonViewedDonor != null) // move to next non viewed donor if exists
                {
                    fvDonors.SelectedItem = SecondOldestNonViewedDonor;
                }
                else // all donors viewed, therefore close
                {
                    DonorPopup.IsOpen = false;
                }
                ScrollGrid(fvDonors.SelectedIndex);
            }

            if (ViewModel.GVRC.BackButton)
            {
                DonorPopup.IsOpen = false;
            }
        }

        private void ScrollGrid(int selectedIndex)
        {
            // get total amount of items per row
            int itemsPerRow = (int)Math.Round(gvDonors.ActualWidth / gvDonors.DesiredWidth, 0, MidpointRounding.AwayFromZero); // 4

            // if big card is pointing to an item on the fourth row or greater, attempt to scroll up one row
            if (selectedIndex >= itemsPerRow * 2)
            {
                gvDonors.ScrollIntoView(gvDonors.Items.ElementAt(selectedIndex - (itemsPerRow * 1)), ScrollIntoViewAlignment.Leading);
            }
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!DonorPopup.IsOpen)
            {
                DonorPopup.IsOpen = true;
            }
        }

        // ensure only 4 rows of data exist
        private void GvDonors_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            gvDonors.ItemHeight = e.NewSize.Height / maxItemsPerColumn;
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

        private void DonorPopup_LayoutUpdated(object sender, object e)
        {
            if (fvDonors.ActualWidth == 0 && fvDonors.ActualHeight == 0)
            {
                return;
            }

            double ActualHorizontalOffset = this.DonorPopup.HorizontalOffset;
            double ActualVerticalOffset = this.DonorPopup.VerticalOffset;

            double NewHorizontalOffset = (Window.Current.Bounds.Width - fvDonors.ActualWidth) / 2;
            double NewVerticalOffset = (Window.Current.Bounds.Height - fvDonors.ActualHeight) / 2;

            if (ActualHorizontalOffset != NewHorizontalOffset || ActualVerticalOffset != NewVerticalOffset)
            {
                this.DonorPopup.HorizontalOffset = NewHorizontalOffset;
                this.DonorPopup.VerticalOffset = NewVerticalOffset;
            }
        }

        private void EscapeInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            DonorPopup.IsOpen = false;
        }

        private void FvDonors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get total amount of items per row
            int colItemCount = (int)Math.Round(gvDonors.ActualWidth / gvDonors.DesiredWidth, 0, MidpointRounding.AwayFromZero);



            gvDonors.ScrollIntoView(e.AddedItems.FirstOrDefault());
        }

        private void RightArrowInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var OldestNonViewedDonor = ViewModel.Donors.Where(a => a.IsViewed == false).OrderBy(a => a.CreatedDate).FirstOrDefault();
            var SecondOldestNonViewedDonor = ViewModel.Donors.Where(a => a.IsViewed == false).OrderBy(a => a.CreatedDate).Skip(1).FirstOrDefault();

            if (!DonorPopup.IsOpen && OldestNonViewedDonor != null) // popup opened and oldest non viewed donor is shown
            {
                fvDonors.SelectedItem = OldestNonViewedDonor;
                DonorPopup.IsOpen = true;
            }
            else if (fvDonors.SelectedItem == OldestNonViewedDonor && SecondOldestNonViewedDonor != null) // move to next non viewed donor if exists
            {
                fvDonors.SelectedItem = SecondOldestNonViewedDonor;
            }
            else // all donors viewed, therefore close
            {
                DonorPopup.IsOpen = false;
            }
            ScrollGrid(fvDonors.SelectedIndex);
            args.Handled = true;
        }
    }
}
