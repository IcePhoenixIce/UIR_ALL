using Newtonsoft.Json;
using System.Text;
using WebAppBlazor.Data.Models;
using System.Security.Cryptography;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;

namespace WebAppBlazor.Services
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

		public async Task<SpecialistName?>  SpecialistAsync(int id)
		{
			var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"Specialists/{id}");
			var token = await _localStorageService.GetItemAsync<string>("tokenA");
			requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			var response = await _httpClient.SendAsync(requstMassage);
			var responseStatusCode = response.StatusCode;
			if (responseStatusCode == System.Net.HttpStatusCode.OK)
			{
				var responseBody = await response.Content.ReadAsStringAsync();
				var returned_user = JsonConvert.DeserializeObject<Specialist?>(responseBody);
				return await Task.FromResult(new SpecialistName(returned_user));
			}
			throw new Exception("ServerError!");
		}

		public async Task<IEnumerable<SpecialistName>?> SpecialistsAsync()
		{
			var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"Specialists");
			var token = await _localStorageService.GetItemAsync<string>("tokenA");
			requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			var response = await _httpClient.SendAsync(requstMassage);
			var responseStatusCode = response.StatusCode;
			if (responseStatusCode == System.Net.HttpStatusCode.OK)
			{
				var responseBody = await response.Content.ReadAsStringAsync();
				var returned_user = JsonConvert.DeserializeObject<List<Specialist>?>(responseBody);
				List<SpecialistName>? ret_List=new List<SpecialistName>();
				foreach (var x in returned_user) 
					ret_List.Add(new SpecialistName(x));
				return await Task.FromResult(ret_List);
			}
			throw new Exception("ServerError!");
		}

        public async Task<ActionResult<(Specialist, IDictionary<DateTime, IEnumerable<RecordService>>)>> GetAppointmentCurrentSpec(int id) 
		{
            var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"Specialists/Shedule/{id}");
            var token = await _localStorageService.GetItemAsync<string>("tokenA");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requstMassage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var returned_user = JsonConvert.DeserializeObject<(Specialist, IDictionary<DateTime, IEnumerable<RecordService>>)?>(responseBody);
                return await Task.FromResult(returned_user);
            }
            throw new Exception("ServerError!");
        }
    }
}
