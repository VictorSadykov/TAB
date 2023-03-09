using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using Telegram.Bot;
using TAB;
using TAB.Configuration;
using TAB.Controllers;
using TAB.Services;

namespace TAB
{
    public class Program
    {
        public static async Task Main()
        {
            // Объект, отвечающий за постоянный жизненный цикл приложения
            var host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                    ConfigureServices(services)) // Задаем конфигурацию
                .UseConsoleLifetime() // Позволяет поддерживать приложение активным в консоли
                .Build(); // Собираем

            Console.WriteLine("Сервис запущен");

            // Запуск сервиса
            await host.RunAsync();
            Console.WriteLine("Сервис остановлен");

            static void ConfigureServices(IServiceCollection services)
            {
                AppSettings appSettings = BuildAppSettings();
                services.AddSingleton(BuildAppSettings());

                // Регистрация сервисов
                services.AddSingleton<IStorage, MemoryStorage>();

                // Регистрация контроллеров
                services.AddTransient<TextMessageController>();
                services.AddTransient<InlineKeyboardController>();
                services.AddTransient<DefaultMessageController>();


                // Регистрируем объект TelegramBotClient c токеном подключения
                services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(
                    appSettings.BotToken // Токен бота
                    ));
                // Регистрируем постоянно активный сервис бота
                services.AddHostedService<Bot>();
            }
        }

        static AppSettings BuildAppSettings()
        {
            return new AppSettings()
            {
                BotToken = "6148168909:AAEeOwBh3EDNmZ3YsD3jnGl48HG1VmBAiAQ",                
            };
        }
    }
}