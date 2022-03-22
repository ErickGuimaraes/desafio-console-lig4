using System;
using System.Threading.Tasks;
using ConsoleLig4.Core.Interfaces;
using ConsoleLig4.Core.Models;
using ConsoleLig4.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleLig4
{
    class Program
    {
        private static int BOARDSIZE = 5; // Min 5;
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            _ = services.AddSingleton<IAIService, AIService>();
            _ = services.AddTransient<IGameService, GameService>();
            _ = services.AddSingleton<IInputService, InputService>();
            _ = services.AddSingleton<IPrintService, PrintService>();
            _ = services.AddSingleton<Configuration>();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            Configuration configuration = serviceProvider.GetService<Configuration>();
            configuration.BoardSize = BOARDSIZE; 

            IGameService gameService = serviceProvider.GetService<IGameService>();
            await gameService.PlayAsync();
        }
    }
}
