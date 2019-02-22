using AngelBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public MainPage()
        {
            ViewModel = new MainPageViewModel();
            this.InitializeComponent();

            AngelPopup.Height = 500;


            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            //Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode();


            coreTitleBar.ExtendViewIntoTitleBar = false;
        }

        public MainPageViewModel ViewModel { get; set; }

        private void AddEditAngel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AngelListView), ViewModel.Angels);
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!AngelPopup.IsOpen) { AngelPopup.IsOpen = true; }
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
    }
}
