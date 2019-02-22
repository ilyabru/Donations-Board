using AngelBoard.Models;
using AngelBoard.Services;
using AngelBoard.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AngelBoard.ViewModels
{
    public class SponsorViewModel : BaseViewModel
    {
        private readonly ISponsorService _sponsorService;

        private ObservableCollection<Sponsor> sponsors;
        private Sponsor selectedSponsor;

        public ObservableCollection<Sponsor> Sponsors
        {
            get => sponsors;
            set => SetPropertyValue(ref sponsors, value);
        }

        public Sponsor SelectedSponsor
        {
            get => selectedSponsor;
            set => SetPropertyValue(ref selectedSponsor, value);
        }

        public SponsorViewModel(ISponsorService sponsorService)
        {
            _sponsorService = sponsorService;
        }

        //public ICommand PickImage => new Command(async () => await OnPickImage());
        //public ICommand SponsorTapped => new Command<Sponsor>(OnSponsorTapped);

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            //Sponsors = await _sponsorService.GetSponsorsAsync();

            //int savedSponsorId = Preferences.Get("selected_sponsor", -1);

            //if (savedSponsorId > -1)
            //    SelectedSponsor = await _sponsorService.GetSponsorAsync(savedSponsorId);

            IsBusy = false;
        }

        private async Task OnPickImage()
        {
            //var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            //{
            //    PhotoSize = PhotoSize.Medium
            //});

            //if (file != null)
            //{
            //    var sponsor = new Sponsor
            //    {
            //        ImagePath = file.Path,
            //        Name = Path.GetFileName(file.Path)
            //    };

            //    await _sponsorService.AddSponsorAsync(sponsor);

            //    Sponsors.Add(sponsor);
            //}
        }

        private void OnSponsorTapped(Sponsor sponsor)
        {
            //Preferences.Set("selected_sponsor", sponsor.Id);

            //MessagingCenter.Send(this, "changeSponsor", sponsor);
        }
    }
}
