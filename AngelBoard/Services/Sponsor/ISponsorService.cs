using AngelBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace AngelBoard.Services
{
    public interface ISponsorService
    {
        Task<ObservableCollection<Sponsor>> GetSponsorsAsync();
        Task<Sponsor> GetSponsorAsync(int sponsorId);
        Task AddSponsorAsync(Sponsor sponsor);
        Task DeleteSponsorAsync(Sponsor sponsor);
    }
}
