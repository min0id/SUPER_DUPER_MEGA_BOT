using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;

namespace SUPER_DUPER_MEGA_BOT;

internal static class TelegramWorker
{
    private static string _pathToApiKey;
    private static string _apiKey;

    private static ITelegramBotClient _botClient;

    private static ManualResetEvent _stopEvent;

    private static void InitializeBotClient() {
        LoadApiKeyFromFile();
        _botClient = new TelegramBotClient(_apiKey);
    }

    private static void LoadApiKeyFromFile() =>
        _apiKey = System.IO.File.ReadAllText(_pathToApiKey);

    public static void Start(string pathToApiKey, ManualResetEvent stopEvent)
    {
        _pathToApiKey = pathToApiKey;
        _stopEvent = stopEvent;

        InitializeBotClient();

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var recieverOptions = new ReceiverOptions
        {
            AllowedUpdates = { },
        };

        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            recieverOptions,
            cancellationToken
        );
    }

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;
            if (message.Text is not null)
            {
                if (message.Text.ToLower() == "/start")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                    return;
                }
                else if (message.Text.ToLower() == "/stop")
                {
                    await botClient.SendTextMessageAsync(message.Chat, "Ну ок");
                    _stopEvent.Set();
                    return;
                }
                await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");
            }
            else {
                await botClient.SendTextMessageAsync(message.Chat, "Какая-то хуйня");
            }
        }
    }

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }

}
