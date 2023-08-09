using Newtonsoft.Json;
using System.Text;
using WebAppBlazor.Data.Models;
using System.Security.Cryptography;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;
using Radzen.Blazor.Rendering;

namespace WebAppBlazor.Services.ServiceA
{
    public class SpecialistService : ISpecialistService
    {
        private HttpClient _httpClient { get; }
        private ILocalStorageService _localStorageService { get; }

        public SpecialistService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<Specialist> SpecialistAsync(int id)
        {
            var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"Specialists/{id}");
            var token = await _localStorageService.GetItemAsync<string>("tokenA");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requstMassage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Specialist>(responseBody);
            }
            throw new Exception("ServerError!");
        }

        public async Task<IEnumerable<Specialist>> SpecialistsAsync()
        {
            var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"Specialists");
            var token = await _localStorageService.GetItemAsync<string>("tokenA");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requstMassage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Specialist>>(responseBody);
            }
            throw new Exception("ServerError!");
        }

        public async Task<IEnumerable<RecordService>> GetAppointmentCurrentSpec(int id)
        {
            var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"Specialists/Shedule/{id}");
            var token = await _localStorageService.GetItemAsync<string>("tokenA");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requstMassage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<RecordService>>(responseBody);
            }
            throw new Exception("ServerError!");
        }

        public async Task<bool> SpecialistPutAsync(int id, Specialist spec)
        {
            string serializeAppointment = JsonConvert.SerializeObject(spec);
            var requstMassage = new HttpRequestMessage(HttpMethod.Put, $"Specialists/{id}");
            var token = await _localStorageService.GetItemAsync<string>("tokenA");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            requstMassage.Content = new StringContent(serializeAppointment);
            requstMassage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json");

            var response = await _httpClient.SendAsync(requstMassage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode == System.Net.HttpStatusCode.NoContent)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var returned_user = JsonConvert.DeserializeObject<Specialist?>(responseBody);
                return true;
            }
            return false;
        }
    }
}
