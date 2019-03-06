using AngelBoard.Models;
using AngelBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AngelBoard.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AngelListView : Page
    {
        public AngelListView()
        {
            ViewModel = ServiceLocator.Current.GetService<AngelListViewModel>();
            this.InitializeComponent();
        }

        public AngelListViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is ObservableCollection<Angel>)
            {
                ViewModel.Angels = (ObservableCollection<Angel>)e.Parameter;
            }
            base.OnNavigatedTo(e);
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void MenuFlyoutItem_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void MenuFlyoutItem_Tapped_1(object sender, TappedRoutedEventArgs e)
        {

        }

        //private void ListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        //{
        //    FrameworkElement senderElement = sender as FrameworkElement;
        //    // If you need the clicked element:
        //    // Item whichOne = senderElement.DataContext as Item;
        //    FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
        //    flyoutBase.ShowAt(senderElement);
        //}

        private void StackPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            // If you need the clicked element:
            // Item whichOne = senderElement.DataContext as Item;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

    }
}
