using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngelBoard.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AngelBoard
{
    public class ServiceLocator
    {
        private void Configure()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IAngelService, AngelService>()
                .BuildServiceProvider();
            
        }
    }
}
