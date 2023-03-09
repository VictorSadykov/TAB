using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace TAB.Controllers
{
    public class DefaultMessageController
    {
        private readonly ITelegramBotClient _telegramClient;

        public DefaultMessageController(ITelegramBotClient telegramClient)
        {
            _telegramClient = telegramClient;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            Console.WriteLine($"Контроллер {this.GetType().Name} получил сообщение");

            await _telegramClient.SendTextMessageAsync(
                message.Chat.Id,
                $"Получено сообщение не поддерживаемого типа",
                cancellationToken: ct
                );


        }
    }
}
