using Newtonsoft.Json;
using System.Text;
using WebAppBlazor.Data.Models;
using System.Security.Cryptography;
using Blazored.LocalStorage;

namespace WebAppBlazor.Services
{
    public class RoomService : IRoomService
	{
        private HttpClient _httpClient { get; }
		private ILocalStorageService _localStorageService { get; }

        public RoomService(HttpClient httpClient, ILocalStorageService localStorageService) 
        { 
            _httpClient = httpClient;
			_localStorageService = localStorageService;
        }

        public async Task<ICollection<Room>?> RoomsAsync(int areaId)
        {
            var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"{areaId}");
            var token = await _localStorageService.GetItemAsync<string>("tokenB");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requstMassage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var returned_user = JsonConvert.DeserializeObject<List<Room>?>(responseBody);
                return await Task.FromResult(returned_user);
            }
            throw new NotImplementedException();
        }

        public async Task<(Room, IDictionary<DateTime, IEnumerable<RecordService>>)?> RoomAsync(int id)
        {
            var requstMassage = new HttpRequestMessage(HttpMethod.Get, $"Room/{id}");
            var token = await _localStorageService.GetItemAsync<string>("tokenB");
            requstMassage.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(requstMassage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var returned_user = JsonConvert.DeserializeObject<(Room, IDictionary<DateTime, IEnumerable<RecordService>>)?>(responseBody);
                return await Task.FromResult(returned_user);
            }
            throw new NotImplementedException();
        }
    }
}
