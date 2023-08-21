using System;
using Azure;
using Azure.AI.Language.QuestionAnswering;
using Azure.AI.TextAnalytics;
using static System.Net.Mime.MediaTypeNames;

//Martin Karlsson NET22 - Labb 1
namespace qnaconsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Keys and Endpoints for the NLP Language Detection side
            string key = "84ed3d5d4fae400583701a45dde5c8c8";
            Uri endpointLang = new Uri("https://cogserviceschatwesteurope.cognitiveservices.azure.com/");

            AzureKeyCredential credentials = new AzureKeyCredential(key);
            TextAnalyticsClient textClient = new TextAnalyticsClient(endpointLang, credentials);

            // Keys and Endpoints for the QnA chatbot side
            Uri endpoint = new Uri("https://langservicewesteurope.cognitiveservices.azure.com/");
            AzureKeyCredential credential = new AzureKeyCredential("7794f2b4164446b4a6839ee5f7edbb3c");
            string projectName = "LearnFAQ";
            string deploymentName = "production";
            QuestionAnsweringClient client = new QuestionAnsweringClient(endpoint, credential);
            QuestionAnsweringProject project = new QuestionAnsweringProject(projectName, deploymentName);

            var question = "";
            while (question.ToLower() !="quit")
            {
                Console.WriteLine("Ask me anything and write quit to exit");
                question = Console.ReadLine();

                //Gets the answer when communicating with the QnA-bot
                Response<AnswersResult> response = client.GetAnswers(question, project);

                //Reads and gets the language depending on what is written
                Response<DetectedLanguage> responseLang = textClient.DetectLanguage(question);
                DetectedLanguage lang = responseLang.Value;


                foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                {
                    Console.WriteLine($"Q:{question}");
                    Console.WriteLine($"A:{answer.Answer}");

                    //When detecting a language it will be displayed here with the help of Azure AI Text Analytics
                    Console.WriteLine();
                    Console.WriteLine("Detected Language:");
                    Console.WriteLine($"{lang.Name} (ISO Code: {lang.Iso6391Name}): {lang.ConfidenceScore:P}");
                }
            }
        }
    }
}