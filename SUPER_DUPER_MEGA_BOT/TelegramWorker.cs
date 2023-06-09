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
        if (_botClient is not null)
            return;

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
                switch (message.Text.ToLower())
                {
                    case "/start":
                        await botClient.SendTextMessageAsync(message.Chat, "Короче, Меченый, я тебя спас и в благородство играть не буду: выполнишь для меня пару заданий — и мы в расчете. Заодно посмотрим, как быстро у тебя башка после амнезии прояснится. А по твоей теме постараюсь разузнать. Хрен его знает, на кой ляд тебе этот Стрелок сдался, но я в чужие дела не лезу, хочешь убить, значит есть за что...");
                        break;
                    case "/stop":
                        await botClient.SendTextMessageAsync(message.Chat, "Ну, удачной охоты, сталкер.");
                        break;
                    default:
                        await botClient.SendTextMessageAsync(message.Chat, "Что за бодягу ты мне приволок?");
                        break;
                }
                return;
            }
            else {
                await botClient.SendTextMessageAsync(message.Chat, "Братан, ты не врубаешься? Мне нужен реальный товар!");
            }
        }
    }

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }

}
