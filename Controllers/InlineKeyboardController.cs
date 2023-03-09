using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TAB.Services;

namespace TAB.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;

        public InlineKeyboardController(ITelegramBotClient telegramClient, IStorage memoryStorage)
        {
            _telegramClient = telegramClient;
            _memoryStorage = memoryStorage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            long chatId = callbackQuery.From.Id;

            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(chatId).CounterMode = callbackQuery.Data;

            string languageText = callbackQuery.Data switch
            {
                "words" => "Подсчёт символов",
                "numbers" => "Подсчёт суммы чисел",
                _ => String.Empty
            };


            await _telegramClient.SendTextMessageAsync(
                chatId,
                $"<b>Режим счётчика - {languageText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Режим можно поменять в главном меню. (/start)",
                cancellationToken: ct,
                parseMode: ParseMode.Html
                );

            string message = _memoryStorage.GetSession(chatId).CounterMode == "words" ? "Напишите любой текст." : "Начните вводить цифры через пробел";

            await _telegramClient.SendTextMessageAsync(
                chatId,
                message,
                cancellationToken: ct
                );
        }
    }
}
