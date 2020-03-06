using DonationBoard.Models;
using DonationBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DonationBoard.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DonorListView : Page
    {
        public DonorListView()
        {
            ViewModel = ServiceLocator.Current.GetService<DonorListViewModel>();
            this.InitializeComponent();
        }

        public DonorListViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Subscribe();
            await ViewModel.LoadAsync();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Unsubscribe();
        }

        private void TxtLocation_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suggestions = SearchControls(sender.Text);

                if (suggestions.Count > 0)
                    sender.ItemsSource = suggestions;
                else
                    sender.IsSuggestionListOpen = false;
            }
        }

        private List<string> SearchControls(string query)
        {
            var suggestions = new List<string>();

            var matchingItems = ViewModel.Locations.Where(
                l => l.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0);

            foreach (var item in matchingItems)
            {
                suggestions.Add(item);
            }

            return suggestions.OrderByDescending(i => 
                i.StartsWith(query, StringComparison.CurrentCultureIgnoreCase))
                    .ThenBy(i => i)
                    .ToList();
        }

        private void lstvDonors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.SelectedDonors = lstvDonors.SelectedItems
                .Cast<Donor>()
                .ToList();
        }

        private void btnClearToggles_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (lstvDonors.SelectionMode == ListViewSelectionMode.Multiple)
            {
                lstvDonors.SelectedItems.Clear();
            }
        }
    }
}
