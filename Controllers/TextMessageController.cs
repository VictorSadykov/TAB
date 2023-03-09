using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAB.Configuration;
using TAB.Services;
using TAB.Utilities;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TAB.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _storage;

        public TextMessageController(ITelegramBotClient telegramClient, IStorage storage)
        {
            _telegramClient = telegramClient;
            _storage = storage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            long chatId = message.Chat.Id;
            string text = message.Text;

            if (text == "/start")
            {
                // Объект представляющий кнопки
                var buttons = new List<InlineKeyboardButton[]>();
                buttons.Add(new[] // добавление кнопок
                {
                        InlineKeyboardButton.WithCallbackData($"Подсчёт символов", "words"),
                        InlineKeyboardButton.WithCallbackData($"Подсчёт суммы чисел", "numbers"),
                    });

                await _telegramClient.SendTextMessageAsync(
                    chatId, // Id чата
                    $"<b>Выберите режим подсчёта</b> {Environment.NewLine}",// Сообщение
                    cancellationToken: ct, // Токен отмены
                    parseMode: ParseMode.Html, // Режим парсинга сообщения
                    replyMarkup: new InlineKeyboardMarkup(buttons) // Дополнительные параметры интерфейса. Обычно, сюда передаётся разметка кнопок
                    );
            }
            else
            {
                if (_storage.GetSession(chatId).CounterMode == "words")
                {
                    await _telegramClient.SendTextMessageAsync(
                        chatId,
                        $"Длина вашего сообщения: {Counter.CountWords(text)} символов.",
                        cancellationToken: ct
                        );

                    await _telegramClient.SendTextMessageAsync(
                        chatId,
                        $"Можете прислать ещё один произвольный текст - в ответ получите количество символов в нём.{Environment.NewLine}" +
                        $"{Environment.NewLine} Для смены режима пропишите \"/start\"",
                        cancellationToken: ct
                        );
                }
                else
                {
                    bool isAllAreNumbers = Counter.IsAllAreNumbers(text);
                    string messageToSend = isAllAreNumbers ?
                        $"Сумма всех чисел: {Counter.CountNumbers(text)}" :
                        "Неправильный тип сообщения. Запишите произвольные целые числа через пробел.";

                    await _telegramClient.SendTextMessageAsync(
                        chatId,
                        messageToSend,
                        cancellationToken: ct
                        );

                    if (isAllAreNumbers)
                    {
                        await _telegramClient.SendTextMessageAsync(
                        chatId,
                        "Можете ввести ещё одно множество чисел или вернуться в меню - /start",
                        cancellationToken: ct
                        );
                    }
                }
            }            
        }
    }
}
