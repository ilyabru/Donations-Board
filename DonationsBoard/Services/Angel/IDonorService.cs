using DonationBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace DonationBoard.Services
{
    public interface IDonorService
    {
        Task<ObservableCollection<Donor>> GetDonorsAsync();
        Task<Donor> GetDonorAsync(int donorId);
        Task AddDonorAsync(Donor donor);
        Task UpdateDonorAsync(Donor donor);
        Task DeleteDonorAsync(Donor donor);

        Task<ObservableCollection<string>> GetLocations();
    }
}
