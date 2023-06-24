using Newtonsoft.Json;
using System.Text;
using WebAppBlazor.Data.Models;
using System.Security.Cryptography;
using Blazored.LocalStorage;
using Radzen.Blazor.Rendering;

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
            var token = await _localStorageService.GetItemAsync<string>("tokenB");
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

        public async Task<IEnumerable<RecordCurrent>> SpecAsync(int id)
        {
            var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"Spec/{id}");
            var token = await _localStorageService.GetItemAsync<string>("tokenB");
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

        public async Task<RecordCurrent?> PostRecordAsync(RecordCurrent record)
        {
            string serializeAppointment = JsonConvert.SerializeObject(record);
            var requstMassage = new HttpRequestMessage(HttpMethod.Post, "User");
            var token = await _localStorageService.GetItemAsync<string>("tokenB");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            requstMassage.Content = new StringContent(serializeAppointment);
            requstMassage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json");
            var response = await _httpClient.SendAsync(requstMassage);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var returned_user = JsonConvert.DeserializeObject<RecordCurrent>(responseBody);
                return await Task.FromResult(returned_user);
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var requstMassage = new HttpRequestMessage(HttpMethod.Delete, $"{id}");
            var token = await _localStorageService.GetItemAsync<string>("tokenB");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requstMassage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode == System.Net.HttpStatusCode.NoContent)
                return true;
            throw new Exception("ServerError!");
        }
    }
}
