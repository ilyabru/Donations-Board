using DonationBoard.Helpers;
using System;

namespace DonationBoard.ViewModels
{
    public class NavigationItem : ObservableObject
    {
        public NavigationItem(string label, Type viewModel)
        {
            Label = label;
            ViewModel = viewModel;
        }

        public readonly string Label;
        public readonly Type ViewModel;
    }
}
