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
        Task<ObservableCollection<Angel>> GetAngelsAsync();
        Task<Angel> GetAngelAsync(int angelId);
        Task AddAngelAsync(Angel angel);
        Task UpdateAngelAsync(Angel angel);
        Task DeleteAngelAsync(Angel angel);

        Task<ObservableCollection<string>> GetLocations();
    }
}
