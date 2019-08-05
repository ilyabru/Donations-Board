using DonationBoard.Helpers;
using DonationBoard.Services;
using System.Threading.Tasks;

namespace DonationBoard.ViewModels.Base
{
    public class BaseViewModel : ObservableObject
    {
        //protected readonly INavigationService NavigationService;

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }

        public BaseViewModel()
        {
            //NavigationService = ViewModelLocator.Resolve<INavigationService>();
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
