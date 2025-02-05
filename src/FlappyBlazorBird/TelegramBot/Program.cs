using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Microsoft.VisualBasic;

class Program
{
    private static readonly ITelegramBotClient botClient = new TelegramBotClient("7566837637:AAGFBGElRRH2XdVgZi4qiuvGw2eA6IDp4j4");

    static async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message.Text == "/start")
        {
            var webAppUrl = "https://192.168.1.146:12050"; // Замени на URL своего веб-приложения
            //var webAppUrl = "https://habr.com/ru/articles/756814/"; // Замени на URL своего веб-приложения

            var webAppInfo = new WebAppInfo { Url = webAppUrl };

            var keyboard = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton("Открыть WebApp") { WebApp = webAppInfo }
            })
            {
                ResizeKeyboard = true
            };

            await bot.SendTextMessageAsync(
                chatId: update.Message.Chat.Id,
                text: "Нажмите кнопку ниже, чтобы открыть веб-приложение:",
                replyMarkup: keyboard,
                cancellationToken: cancellationToken
            );
        }
    }

    static Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ошибка: {exception.Message}");
        return Task.CompletedTask;
    }

    static async Task Main()
    {
        using var cts = new CancellationTokenSource();

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // Получаем все обновления
        };

        botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            errorHandler: HandleErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );


        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Бот {me.Username} запущен!");
        Console.ReadLine();

        cts.Cancel();
    }
}
