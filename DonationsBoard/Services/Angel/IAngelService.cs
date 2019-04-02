using AngelBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace AngelBoard.Services
{
    public interface IAngelService
    {
        Task<ObservableCollection<Donor>> GetAngelsAsync();
        Task<Donor> GetAngelAsync(int angelId);
        Task AddAngelAsync(Donor angel);
        Task UpdateAngelAsync(Donor angel);
        Task DeleteAngelAsync(Donor angel);

        Task<ObservableCollection<string>> GetLocations();
    }
}
