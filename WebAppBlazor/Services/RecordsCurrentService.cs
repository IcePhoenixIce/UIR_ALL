using Newtonsoft.Json;
using System.Text;
using WebAppBlazor.Data.Models;
using System.Security.Cryptography;
using Blazored.LocalStorage;

namespace WebAppBlazor.Services
{
    public class RecordsCurrentService : IRecordsCurrentService
    {
        public HttpClient _httpClient { get; }
        private ILocalStorageService _localStorageService { get; }
        public RecordsCurrentService(HttpClient httpClient, ILocalStorageService localStorageService) 
        { 
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<IEnumerable<RecordCurrent>> UserAsync(int id)
        {
            var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"User/{id}");
            var token = await _localStorageService.GetItemAsync<string>("token");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requstMassage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var returned_user = JsonConvert.DeserializeObject<List<RecordCurrent>?>(responseBody);
                return await Task.FromResult(returned_user);
            }
            throw new Exception("ServerError!");
        }
    }
}
