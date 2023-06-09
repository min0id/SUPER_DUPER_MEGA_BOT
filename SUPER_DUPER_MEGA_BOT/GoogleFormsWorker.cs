using Google.Apis.Auth.OAuth2;
using Google.Apis.Forms.v1;
using Google.Apis.Forms.v1.Data;
using Google.Apis.Services;

namespace SUPER_DUPER_MEGA_BOT;

internal static class GoogleFormsWorker
{
    private static string _pathToSecret;
    private static string _pathToFormID;

    private static string _formId;
    
    private static GoogleCredential _credential;
    private static FormsService _formsService;

    private static Form _form;
    private static ListFormResponsesResponse _responses;

    public static void Start(string pathToFormId, string pathToSecret)
    {
        _pathToSecret = pathToSecret;
        _pathToFormID = pathToFormId;

        LoadFormIdFromFile();
        InitializeFormService();
        LoadForm();
        LoadResponses();
    }

    private static void InitializeFormService()
    {
        using (var stream = new FileStream(_pathToSecret, FileMode.Open, FileAccess.Read))
        {
            _credential = GoogleCredential.FromStream(stream);
        }

        _formsService = new FormsService(new BaseClientService.Initializer
        {
            ApplicationName = "SUPER_DUPER_MEGA_BOT",
            HttpClientInitializer = _credential,
        });
    }

    private static void LoadForm() =>
        _form = _formsService.Forms.Get(_formId).Execute();

    private static void LoadResponses() =>
        _responses = _formsService.Forms.Responses.List(_formId).Execute();

    private static void LoadFormIdFromFile() =>
        _formId = File.ReadAllText(_pathToFormID);

    public static void Print()
    {
        foreach (var item in _form.Items)
            Console.WriteLine($"{item.QuestionItem.Question.QuestionId} - {item.Title}");

        Console.WriteLine();
        Console.WriteLine();

        foreach (var response in _responses.Responses)
            foreach (var answer in response.Answers)
            {
                Console.WriteLine($"{answer.Value.QuestionId}");
                foreach (var ans in answer.Value.TextAnswers.Answers)
                    Console.WriteLine($"\t{ans.Value}");
            }
    }

    public static void PrintAsJson()
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(_form));
        Console.WriteLine();
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(_responses));
    }
}
