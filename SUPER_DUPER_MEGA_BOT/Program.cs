namespace SUPER_DUPER_MEGA_BOT;

internal class Program
{
    static ManualResetEvent telegramWorkerStopEvent = new ManualResetEvent(false);

    static void Main()
    {
        GoogleFormsWorker.Start("secrets/FormID.txt", "secrets/service_account_secret.json");
        GoogleFormsWorker.PrintAsJson();

        TelegramWorker.Start("secrets/telegram_bot_api_key.txt", telegramWorkerStopEvent);

        telegramWorkerStopEvent.WaitOne();
    }
}