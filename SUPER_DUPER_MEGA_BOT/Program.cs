namespace SUPER_DUPER_MEGA_BOT
{
    internal class Program
    {
        static void Main()
        {
            var formWorker = new GoogleFormsWorker("secrets/FormID.txt", "secrets/service_account_secret.json");
            formWorker.Print();
        }
    }
}