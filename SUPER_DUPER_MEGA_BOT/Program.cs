namespace SUPER_DUPER_MEGA_BOT
{
    internal class Program
    {
        static void Main()
        {
            var formWorker = new GoogleFormsWorker("FormID.txt", "service_account_secret.json");
            formWorker.Print();
        }
    }
}