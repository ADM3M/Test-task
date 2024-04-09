using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace testTask.client
{
    public static class Program
    {
        public static async Task SendUsers()
        {
            var mockData = await ReadMockData();
            var content = new StringContent(mockData, Encoding.UTF8, MediaTypeNames.Application.Json);

            using var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:8080/api/patient/create");

            var result = await client.PostAsync("", content);
            if (result.IsSuccessStatusCode)
            {
                Console.WriteLine($"Request succeed with {result.StatusCode}");
            }
            else
            {
                Console.WriteLine($"Request failed with {result.StatusCode}");
            }
        }

        public static async Task<string> ReadMockData()
        {
            try
            {
                return await File.ReadAllTextAsync(@"..\..\..\MockUsers.json");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while reading data from a file");
                throw;
            }
        }

        public static async Task Main(string[] args)
        {
            await SendUsers();
        }
    }
}
