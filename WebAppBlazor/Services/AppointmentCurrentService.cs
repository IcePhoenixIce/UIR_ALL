using Newtonsoft.Json;
using System.Text;
using WebAppBlazor.Data.Models;
using System.Security.Cryptography;
using Blazored.LocalStorage;

namespace WebAppBlazor.Services
{
    public class AppointmentCurrentService : IAppointmentCurrentService
	{
        private HttpClient _httpClient { get; }
		private ILocalStorageService _localStorageService { get; }

        public AppointmentCurrentService(HttpClient httpClient, ILocalStorageService localStorageService) 
        { 
            _httpClient = httpClient;
			_localStorageService = localStorageService;
        }

		public async Task<IEnumerable<AppointmentCurrent>?> UserAsync(int id) 
		{
			var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"User/{id}");
			var token = await _localStorageService.GetItemAsync<string>("tokenA");
			requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			var response = await _httpClient.SendAsync(requstMassage);
			var responseStatusCode = response.StatusCode;
			if (responseStatusCode == System.Net.HttpStatusCode.OK)
			{
				var responseBody = await response.Content.ReadAsStringAsync();
				var returned_user = JsonConvert.DeserializeObject<List<AppointmentCurrent>?>(responseBody);
				return await Task.FromResult(returned_user);
			}
			throw new Exception("ServerError!");
		}

		public async Task<IEnumerable<AppointmentCurrent>?> SpecialistAsync(int id)
		{
			var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"Specialist/{id}");
			var token = await _localStorageService.GetItemAsync<string>("tokenA");
			requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			var response = await _httpClient.SendAsync(requstMassage);
			var responseStatusCode = response.StatusCode;
			if (responseStatusCode == System.Net.HttpStatusCode.OK)
			{
				var responseBody = await response.Content.ReadAsStringAsync();
				var returned_user = JsonConvert.DeserializeObject<List<AppointmentCurrent>?>(responseBody);
				return await Task.FromResult(returned_user);
			}
			throw new Exception("ServerError!");
		}

        public async Task<AppointmentCurrent?> PostAppointmentAsync(AppointmentCurrent appointment)
        {
            string serializeAppointment = JsonConvert.SerializeObject(appointment);
            var requstMassage = new HttpRequestMessage(HttpMethod.Post, "User");
            var token = await _localStorageService.GetItemAsync<string>("tokenA");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            requstMassage.Content = new StringContent(serializeAppointment);
            requstMassage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json");
            var response = await _httpClient.SendAsync(requstMassage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode == System.Net.HttpStatusCode.Created)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var returned_user = JsonConvert.DeserializeObject<AppointmentCurrent>(responseBody);
                return await Task.FromResult(returned_user);
            }
            return null;
        }
    }
}
