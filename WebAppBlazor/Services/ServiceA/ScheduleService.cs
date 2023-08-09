using Newtonsoft.Json;
using System.Text;
using WebAppBlazor.Data.Models;
using System.Security.Cryptography;
using Blazored.LocalStorage;

namespace WebAppBlazor.Services.ServiceA
{
    public class ScheduleService : IScheduleService
    {
        private HttpClient _httpClient { get; }
        private ILocalStorageService _localStorageService { get; }

        public ScheduleService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<bool> ShedulePutAsync(int id, SheduleTable shedule)
        {
            string serializeShedule = JsonConvert.SerializeObject(shedule);
            var requstMassage = new HttpRequestMessage(HttpMethod.Put, $"{id}");
            var token = await _localStorageService.GetItemAsync<string>("tokenA");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            requstMassage.Content = new StringContent(serializeShedule);
            requstMassage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json");
            var response = await _httpClient.SendAsync(requstMassage);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var returned_user = JsonConvert.DeserializeObject<RecordCurrent>(responseBody);
                return true;
            }
            return false;
        }

        public async Task<bool> ShedulePostAsync(SheduleTable shedule)
        {
            string serializeShedule = JsonConvert.SerializeObject(shedule);
            var requstMassage = new HttpRequestMessage(HttpMethod.Post, $"");
            var token = await _localStorageService.GetItemAsync<string>("tokenA");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            requstMassage.Content = new StringContent(serializeShedule);
            requstMassage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json");
            var response = await _httpClient.SendAsync(requstMassage);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var returned_user = JsonConvert.DeserializeObject<RecordCurrent>(responseBody);
                return true;
            }
            return false;
        }

        public async Task<bool> SheduleDeleteAsync(int SpecId, int WeekId)
        {
            var requstMassage = new HttpRequestMessage(HttpMethod.Delete, $"{SpecId}/{WeekId}");
            var token = await _localStorageService.GetItemAsync<string>("tokenA");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            //requstMassage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json");
            var response = await _httpClient.SendAsync(requstMassage);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var returned_user = JsonConvert.DeserializeObject<RecordCurrent>(responseBody);
                return true;
            }
            return false;
        }
    }
}
