using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace BlazorWASM.Services
{
    public class APIService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://opgaver.mercantec.tech/api";

        public APIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BackendStatus?> GetBackendStatusAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseUrl}/Status/all");

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return JsonSerializer.Deserialize<BackendStatus>(jsonString, options);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved tjek af backend status: {ex.Message}");
                return null;
            }
        }
    }

    public class BackendStatus
    {
        public ServerStatus? Server
        {
            get; set;
        }
        public DatabaseStatus? MongoDB
        {
            get; set;
        }
        public DatabaseStatus? PostgreSQL
        {
            get; set;
        }
        public DateTime Timestamp
        {
            get; set;
        }
    }

    public class ServerStatus
    {
        public string Status { get; set; } = string.Empty;
    }

    public class DatabaseStatus
    {
        public string Status { get; set; } = string.Empty;
        public string? Database
        {
            get; set;
        }
        public string? Error
        {
            get; set;
        }
        public bool IsError
        {
            get; set;
        }
    }

}
