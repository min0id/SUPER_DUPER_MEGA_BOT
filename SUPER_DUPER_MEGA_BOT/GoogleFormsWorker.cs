using Google.Apis.Auth.OAuth2;
using Google.Apis.Forms.v1;
using Google.Apis.Forms.v1.Data;
using Google.Apis.Services;

namespace SUPER_DUPER_MEGA_BOT
{
    internal class GoogleFormsWorker
    {
        private string pathToSecret;
        private string pathToFormID;

        private string formId;
        
        private GoogleCredential credential;
        private FormsService formsService;

        private Form form;
        ListFormResponsesResponse responses;

        public GoogleFormsWorker(string pathToFormId, string pathToSecret)
        {
            this.pathToSecret = pathToSecret; 
            this.pathToFormID = pathToFormId;

            GetFormIdFromFile();
            InitializeFormService();
            GetForm();
            GetResponses();
        }

        private void InitializeFormService()
        {
            using (var stream = new FileStream(pathToSecret, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream);
            }

            formsService = new FormsService(new BaseClientService.Initializer
            {
                ApplicationName = "SUPER_DUPER_MEGA_BOT",
                HttpClientInitializer = credential,
            });
        }

        private void GetForm()
        {
            form = formsService.Forms.Get(formId).Execute();
        }

        private void GetResponses()
        {
            responses = formsService.Forms.Responses.List(formId).Execute();
        }

        private void GetFormIdFromFile()
        {
            formId = File.ReadAllText(pathToFormID);
        }

        public void Print()
        {
            foreach (var item in form.Items)
            {
                Console.WriteLine($"{item.QuestionItem.Question.QuestionId} - {item.Title}");
            }
            Console.WriteLine();
            Console.WriteLine();
            foreach (var response in responses.Responses)
            {
                foreach (var answer in response.Answers) {
                    Console.WriteLine($"{answer.Value.QuestionId}");
                    foreach (var ans in answer.Value.TextAnswers.Answers)
                    {
                        Console.WriteLine($"\t{ans.Value}");
                    }
                }
            }
        }
    }
}
