using Google.Apis.Auth.OAuth2;
using Google.Apis.Forms.v1;
using Google.Apis.Forms.v1.Data;
using Google.Apis.Services;

namespace SUPER_DUPER_MEGA_BOT;

internal class GoogleFormsWorker
{
    private string _pathToSecret;
    private string _pathToFormID;

    private string _formId;
    
    private GoogleCredential _credential;
    private FormsService _formsService;

    private Form _form;
    private ListFormResponsesResponse responses;

    public GoogleFormsWorker(string pathToFormId, string pathToSecret)
    {
        _pathToSecret = pathToSecret; 
        _pathToFormID = pathToFormId;

        LoadFormIdFromFile();
        InitializeFormService();
        LoadForm();
        LoadResponses();
    }

    private void InitializeFormService()
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

    private void LoadForm() =>
        _form = _formsService.Forms.Get(_formId).Execute();

    private void LoadResponses() =>
        responses = _formsService.Forms.Responses.List(_formId).Execute();

    private void LoadFormIdFromFile() =>
        _formId = File.ReadAllText(_pathToFormID);

    public void Print()
    {
        foreach (var item in _form.Items)
            Console.WriteLine($"{item.QuestionItem.Question.QuestionId} - {item.Title}");

        Console.WriteLine();
        Console.WriteLine();

        foreach (var response in responses.Responses)
            foreach (var answer in response.Answers)
            {
                Console.WriteLine($"{answer.Value.QuestionId}");
                foreach (var ans in answer.Value.TextAnswers.Answers)
                    Console.WriteLine($"\t{ans.Value}");
            }
        }
    }
