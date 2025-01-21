using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ChatGPTBasedBot
{
    internal class ZapierIntegration
    {
        private readonly string _webhookUrl;

        public ZapierIntegration(string webhookUrl)
        {
            _webhookUrl = webhookUrl;
        }

        public async Task PostToZapierAsync(string message, byte[] imageData, string fileName)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(message), "message");

                    var imageContent = new ByteArrayContent(imageData);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                    content.Add(imageContent, "file", fileName);

                    var response = await client.PostAsync(_webhookUrl, content);
                    response.EnsureSuccessStatusCode();
                }
            }
        }
    }
}
