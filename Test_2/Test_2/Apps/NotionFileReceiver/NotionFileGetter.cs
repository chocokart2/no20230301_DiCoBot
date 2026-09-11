using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2.Apps.NotionFileReceiver
{
    internal class NotionFileGetter
    {
        private readonly HttpClient client;

        public NotionFileGetter()
        {
            client = new HttpClient();
            //client.BaseAddress = new Uri("https://api.notion.com/v1/");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenReceiver.GetApiKey(EKeyType.NotionChocokartApi));
            client.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");
        }

        public async Task<string> GetBlocksAsync()
        {
            Console.WriteLine($">> NotionFileGetter.GetBlocksAsync() : 호출");

            var payload = "{\"page_size\":100}";
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.notion.com/v1/search", content);
            response.EnsureSuccessStatusCode();

            Console.WriteLine($">> NotionFileGetter.GetBlocksAsync() : await client.PostAsync");

            string resultValue = await response.Content.ReadAsStringAsync();

            Console.WriteLine($">> NotionFileGetter.GetBlocksAsync() : await response.Content.ReadAsStringAsync();");

            return resultValue;
        }
    }
}
