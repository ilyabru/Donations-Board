﻿using AngelBoard.Models;
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

            return suggestions.OrderByDescending(i => i.StartsWith(query, StringComparison.CurrentCultureIgnoreCase)).ThenBy(i => i).ToList();
        }

    }
}
