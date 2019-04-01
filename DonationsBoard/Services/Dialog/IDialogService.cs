using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngelBoard.Services
{
    public interface IDialogService
    {
        Task ShowAsync(string title, Exception ex, string ok = "Ok");
        Task<bool> ShowAsync(string title, string content, string ok = "Ok", string cancel = null);
    }
}
