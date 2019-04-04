using AngelBoard;
using AngelBoard.ViewModels;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AngelBoard.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StatsView : Page
    {
        public StatsView()
        {
            ViewModel = ServiceLocator.Current.GetService<StatsViewModel>();
            this.InitializeComponent();
        }

        public StatsViewModel ViewModel { get; set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //ViewModel.Subscribe();
            await ViewModel.LoadAsync();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
           // ViewModel.Unsubscribe();
        }
    }
}
