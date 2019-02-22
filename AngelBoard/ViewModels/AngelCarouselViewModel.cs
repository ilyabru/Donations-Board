using AngelBoard.Models;
using AngelBoard.Services;
using AngelBoard.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AngelBoard.ViewModels
{
    public class AngelCarouselViewModel : BaseViewModel
    {
        private IAngelService _angelService;

        private ObservableCollection<Angel> angels;
        private Angel selectedAngel;
        private int selectedIndex;


        public ObservableCollection<Angel> Angels
        {
            get => angels;
            set => SetPropertyValue(ref angels, value);
        }

        public Angel SelectedAngel
        {
            get
            {
                return selectedAngel;
            }
            set
            {   
                if (value != null)
                    value.IsViewed = true;
                SetPropertyValue(ref selectedAngel, value);
            }
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
                // return selectedIndex;
                //if (SelectedAngel != null)
                //    return Angels.IndexOf(SelectedAngel);
                //else
                //    return -1;
                //if (Angels != null && SelectedAngel != null)
                //    return Angels.IndexOf(SelectedAngel);
                //else
                //    return -1;                //return Angels.IndexOf(a => a.IsViewed == false);
            }
            set
            {
                if (selectedAngel == null && Angels != null && selectedIndex == 0)
                    SelectedAngel = Angels.FirstOrDefault();
                SetPropertyValue(ref selectedIndex, value);
            }
        }

        //public ICommand AngelSelected => new Command(async () => await OnAngelSelected());

        public AngelCarouselViewModel(IAngelService angelService)
        {
            _angelService = angelService;
        }

        public override Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            Angels = (ObservableCollection<Angel>)navigationData;
            //SelectedAngel = Angels.FirstOrDefault();

            var firstNonViewedAngel = Angels.FirstOrDefault(a => a.IsViewed == false);
            if (firstNonViewedAngel != null)
                SelectedIndex = Angels.IndexOf(firstNonViewedAngel);
            else
                SelectedIndex = Angels.IndexOf(Angels.LastOrDefault());

            //SelectedIndex = SelectedIndex == 0 ? -1 : SelectedIndex;

            IsBusy = false;

            return base.InitializeAsync(navigationData);
        }

        private async Task OnAngelSelected()
        {
            //if (isFirstAngelViewed)
            //{
            //    //IsBusy = true;
            //    isFirstAngelViewed = false;
            //    SelectedAngel = Angels.FirstOrDefault(a => a.IsViewed == false);
            //   // IsBusy = false;

            //}
           // SelectedAngel.IsViewed = true;
        }
    }
}
