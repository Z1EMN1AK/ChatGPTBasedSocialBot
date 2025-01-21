using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ChatGPTBasedBot;

namespace ChatGPTBasedBot
{
    internal class OpenAiIntegrationClass
    {
        private static readonly HttpClient client = new HttpClient();
        private static string apiKey = "YOUR_OPENAI_API_KEY";

        public OpenAiIntegrationClass(string yourApiKey, string model)
        {
            apiKey = yourApiKey;
            model = model;
        }

        public static async Task<string> GeneratePost(string topic)
        {
            var requestBody = new
            {
                messages = new[]
                {
                        new { role = "system", content = "You are a helpful assistant." },
                        new { role = "user", content = $"Write a post encouraging the purchase of my product in 150 words: {topic}" }
                    },
                max_tokens = 1000 // Zwiększono limit tokenów
            };

            if (!client.DefaultRequestHeaders.Contains("Authorization"))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            }

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadFromJsonAsync<OpenAIChatResponse>();
                        return responseBody.choices[0].message.content.Trim();
                    }
                    if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                    {
                        await Task.Delay((int)Math.Pow(2, i) * 2000);
                    }
                    else
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request error: {ex.Message}");
                    if (i == 4) // throw exception after multiple attempts
                    {
                        throw new HttpRequestException("Failed to generate post after multiple attempts due to request errors.", ex);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                    throw; // Throw exception for unexpected errors
                }
            }
            throw new HttpRequestException("Failed to generate post after multiple attempts due to rate limiting.");
        }
    }

    public class OpenAIChatResponse
    {
        public ChatChoice[] choices { get; set; }
    }

    public class ChatChoice
    {
        public ChatMessage message { get; set; }
    }

    public class ChatMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class OpenAIImageResponse
    {
        public ImageData[] data { get; set; }
    }

    public class ImageData
    {
        public string url { get; set; }
    }

    class ChatGPT
    {
        private static string key;
        OpenAiIntegrationClass openAiAPI;

        public ChatGPT(string key, string model)
        {
            openAiAPI = new OpenAiIntegrationClass(key, model);
        }

        public async Task<string> Run(string msg, string model)
        {
            var post = await OpenAiIntegrationClass.GeneratePost(msg);

            using (StreamWriter sw = new StreamWriter("post.txt"))
            {
                await sw.WriteLineAsync(post);
            }

            Console.WriteLine(post);

            return post;
        }


        public static async Task RunChat(configData config)
        {
            ChatGPT chatGPT = new ChatGPT(config.apiKey, config.chatGPTModel);

            FileFetcher fileFetcher = new FileFetcher();

            fileFetcher.FetchProdAndImg();
            fileFetcher.Fill();

            string msg = "Nazwa: " + fileFetcher.subject + "\n opis: " + fileFetcher.data;

            if (fileFetcher.price != 0)
            {
                msg += "\n Cena: " + fileFetcher.price.ToString();
            }

            string answer = await chatGPT.Run(msg, config.chatGPTModel);

            answer += "\n\n" + fileFetcher.link;

            Console.WriteLine("Link: " + fileFetcher.link);

            // Post through Zapier
            string photoUrl = fileFetcher.imagePath;
            var zapierIntegration = new ZapierIntegration(config.webhookUrl);
            string fileName = Path.GetFileName(photoUrl);
            byte[] imageData = await File.ReadAllBytesAsync(photoUrl);
            await zapierIntegration.PostToZapierAsync(answer, imageData, fileName);
        }
    }
}
