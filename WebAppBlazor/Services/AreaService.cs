using Newtonsoft.Json;
using System.Text;
using WebAppBlazor.Data.Models;
using System.Security.Cryptography;
using Blazored.LocalStorage;

namespace WebAppBlazor.Services
{
    public class AreaService : IAreaService
	{
        private HttpClient _httpClient { get; }
		private ILocalStorageService _localStorageService { get; }

        public AreaService(HttpClient httpClient, ILocalStorageService localStorageService) 
        { 
            _httpClient = httpClient;
			_localStorageService = localStorageService;
        }

		public async Task<IEnumerable<Area>?> AreasAsync() 
		{
			var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"Areas");
			var token = await _localStorageService.GetItemAsync<string>("tokenB");
			requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			var response = await _httpClient.SendAsync(requstMassage);
			var responseStatusCode = response.StatusCode;
			if (responseStatusCode == System.Net.HttpStatusCode.OK)
			{
				var responseBody = await response.Content.ReadAsStringAsync();
				var returned_user = JsonConvert.DeserializeObject<List<Area>?>(responseBody);
				return await Task.FromResult(returned_user);
			}
			throw new Exception("ServerError!");
		}

		public async Task<Area?> AreaAsync(int id)
		{
			var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"Areas/{id}");
			var token = await _localStorageService.GetItemAsync<string>("tokenB");
			requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			var response = await _httpClient.SendAsync(requstMassage);
			var responseStatusCode = response.StatusCode;
			if (responseStatusCode == System.Net.HttpStatusCode.OK)
			{
				var responseBody = await response.Content.ReadAsStringAsync();
				var returned_user = JsonConvert.DeserializeObject<Area?>(responseBody);
				return await Task.FromResult(returned_user);
			}
			throw new Exception("ServerError!");
		}
	}
}
