using AngelBoard.Services;
using AngelBoard.ViewModels;
using GearVrController4Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AngelBoard.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly GearVrController gvc;

        public MainPage()
        {
            ViewModel = ServiceLocator.Current.GetService<MainPageViewModel>();

            InitializeComponent();
            InitlializeContext();
            InitializeNavigation();

            gvc = ServiceLocator.Current.GetService<GearVrController>();
            gvc.PropertyChanged += Gvc_PropertyChanged;
            

            AngelPopup.Height = 500;


            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            //Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode();


            coreTitleBar.ExtendViewIntoTitleBar = false;
        }

        private void Gvc_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (gvc.TouchpadButton == true)
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

            //var savedDeviceInfo = await DeviceInformation.CreateFromIdAsync("BluetoothLE#BluetoothLE5c:f3:70:87:c6:ee-7d:d0:15:ba:ba:2c"); // TODO: save this somewhere, don't hardcode
            //gvc.Create(savedDeviceInfo);
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

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var transform = Window.Current.Content.TransformToVisual(AngelPopup);
            Point point = transform.TransformPoint(new Point(0, 0)); // gets the window's (0,0) coordinate relative to the popup

            //double hOffset = (Window.Current.Bounds.Width - this.ActualWidth) / 2;
            double vOffset = (Window.Current.Bounds.Height - AngelPopup.ActualHeight) / 2;

            //AngelPopup.HorizontalOffset = point.X + hOffset;
            AngelPopup.VerticalOffset = point.Y + vOffset;
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
    }
}
