using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace DonationBoard.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowAsync(string title, Exception ex, string ok = "Ok")
        {
            await ShowAsync(title, ex.Message, ok);
        }

        public async Task<bool> ShowAsync(string title, string content, string ok = "Ok", string cancel = null)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = ok
            };
            if (cancel != null)
            {
                dialog.SecondaryButtonText = cancel;
            }
            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }
    }
}
