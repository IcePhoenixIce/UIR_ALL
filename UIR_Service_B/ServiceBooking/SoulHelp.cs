using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System;
using System.Globalization;
using System.Security.Policy;
using UIR_Service_B.Models;
using static System.Net.WebRequestMethods;

namespace UIR_Service_B.ServiceBooking
{
	public class SoulHelp
	{
		const int days = 7;

		static Dictionary<string, string> data = new Dictionary<string, string>();

		public static async Task<IEnumerable<RecordService>> SoulHelpRecordsServices(string placeId, int roomId, string roomIdSH)
		{
			List<RecordService> recordsServices = new List<RecordService>(7);
			DateTime date = DateTime.Now.Date;
			try
			{
				JObject response = await SoulHelp_RequestApp();
				string appId = response.Value<string>("id");
				string appSecret = response.Value<string>("appSecret");

				///room request
				var builder = new UriBuilder("https://api.soulhelp.ru/v1/widget/room/");
				builder.Query = $"placeId={placeId}";
				string url = builder.Uri.ToString();

				JArray costs = (JArray)((JObject)(await SoulHelp_RequestGet(url, appId, appSecret)).SelectToken($"$..rooms[?(@.id == '{roomIdSH}')]"))["costs"];

				//Суббота
				JArray day6 = new JArray(costs.Where(c => (int)c["day"] == 6));

				//Воскресенье
				JArray day7 = new JArray(costs.Where(c => (int)c["day"] == 7));

				//Будни
				JArray day8 = new JArray(costs.Where(c => (int)c["day"] == 8));

				///order request
				builder = new UriBuilder("https://api.soulhelp.ru/v1/widget/order/");
				builder.Query = $"placeId={placeId}";
				url = builder.Uri.ToString();

				List<JToken> orders = (await SoulHelp_RequestGet(url, appId, appSecret))["orders"].Where(o => (string)o["room"]["id"] == roomIdSH).OrderBy(o => (DateTime)o["start"]).ToList();

				for (int i = 0; i < days; i++)
				{
					//create RecordService for room in i day with prices
					switch (date.DayOfWeek)
					{
						//Воскресенье
						case DayOfWeek.Sunday:
							recordsServices.AddRange(PricesServiceSoulHelp(date, roomId, day7));
							break;
						//Суббота
						case DayOfWeek.Saturday:
							recordsServices.AddRange(PricesServiceSoulHelp(date, roomId, day6));
							break;
						//Будни
						default:
							var res = PricesServiceSoulHelp(date, roomId, day8);
							while (i < days && (date.DayOfWeek != DayOfWeek.Saturday || date.DayOfWeek != DayOfWeek.Sunday))
							{
								recordsServices.AddRange(PricesServiceSoulHelp(date, roomId, day8));
								i++;
								date = date.AddDays(1);
							}
							break;
					}
					date = date.AddDays(1);
				}

				int j = 0;
				if (orders.Count > 0)
				{
					DateTime start = ((DateTime)orders[0]["start"]).AddHours(3);
					DateTime end = ((DateTime)orders[0]["end"]).AddHours(3);

					foreach (var r in recordsServices)
					{
						if (r.From1 >= start)
						{
							if (r.To1 <= end)
							{
								r.IsBooked = true;
							}
							else if (orders.Count > ++j)
							{
								start = ((DateTime)orders[j]["start"]).AddHours(3);
								end = ((DateTime)orders[j]["end"]).AddHours(3);
								if (r.From1 >= start && r.To1 <= end)
								{
									r.IsBooked = true;
								}
							}
							else
								return recordsServices;
						}
					}
				}
				return recordsServices;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}


		}

		public static List<RecordService> PricesServiceSoulHelp(DateTime date, int roomId, JArray prices)
		{
			var result = new List<RecordService>();

			foreach (JToken token in prices)
			{
				TimeSpan timeStart = DateTime.ParseExact(token["timeFrom"].ToString(), "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
				TimeSpan timeEnd = DateTime.ParseExact(token["timeTo"].ToString(), "HH:mm", CultureInfo.InvariantCulture).TimeOfDay;
				DateTime curr = date + timeStart;

				while (curr.TimeOfDay < timeEnd)
				{
					result.Add(new RecordService() { RoomID = roomId, From1 = curr, To1 = curr.AddMinutes(30), IsBooked = false, Price = token["cost"].ToObject<decimal>() / 2 });
					curr = curr.AddMinutes(30);
				}
			}
			return result;

		}

		public static async Task<JObject> SoulHelp_RequestApp()
		{
			using var client = new HttpClient();

			var request = new HttpRequestMessage(
				HttpMethod.Get,
				$"https://api.soulhelp.ru/app");

			request.Headers.Add("Accept", "application/json, text/plain, */*");
			request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
			request.Headers.Referrer = new Uri("https://app.soulhelp.ru/");
			try
			{
				var response = await client.SendAsync(request);
				if (!response.IsSuccessStatusCode)
					throw new Exception("INVALID REQUEST");
				return JObject.Parse(await response.Content.ReadAsStringAsync());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}
		}

		private static async Task<JObject> SoulHelp_RequestGet(string url, string appId, string appSecret)
		{
			using var client = new HttpClient();

			var request = new HttpRequestMessage(
				HttpMethod.Get,
				url);

			request.Headers.Add("Accept", "application/json, text/plain, */*");
			request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
			request.Headers.Add("Booking-App-Id", appId);
			request.Headers.Add("Booking-App-Secret", appSecret);
			request.Headers.Referrer = new Uri("https://app.soulhelp.ru/");
			try
			{
				var response = await client.SendAsync(request);
				return JObject.Parse(await response.Content.ReadAsStringAsync());

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}
		}

		private static async Task<JObject> SoulHelp_RequestPost(string url, string appId, string appSecret, string token)
		{
			using var client = new HttpClient();

			var content = new FormUrlEncodedContent(data);
			var contentString = await content.ReadAsStringAsync();

			var request = new HttpRequestMessage(
				HttpMethod.Post,
				url);
			request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
			request.Headers.Add("Accept", "application/json, text/plain, */*");
			request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
			request.Headers.Add("Booking-App-Id", appId);
			request.Headers.Add("Booking-App-Secret", appSecret);
			request.Content = content;
			request.Headers.Referrer = new Uri("https://soulhelp.ru/");
			var response = await client.SendAsync(request);
			data.Clear();
			try
			{
				return JObject.Parse(await response.Content.ReadAsStringAsync());

			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}
		}

		public static async Task<JObject> SoulHelpLogin(string appId, string appSecret, PassGarmony passGarmony) 
		{
			data["phone"] = passGarmony.Login;
			data["password"] = passGarmony.Password;
			using var client = new HttpClient();

			var content = new FormUrlEncodedContent(data);
			var contentString = await content.ReadAsStringAsync();

			var request = new HttpRequestMessage(
				HttpMethod.Post,
				"https://api.soulhelp.ru/v1/auth/login");
			request.Headers.Add("Accept", "application/json, text/plain, */*");
			request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
			request.Headers.Add("Booking-App-Id", appId);
			request.Headers.Add("Booking-App-Secret", appSecret);
			request.Content = content;
			request.Headers.Referrer = new Uri("https://soulhelp.ru/");
			var response = await client.SendAsync(request);
			data.Clear();
			try
			{
				return JObject.Parse(await response.Content.ReadAsStringAsync());
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return null;
			}
		}

		public static async Task<string> SoulHelpBooking(string appId, string appSecret, string token, RecordCurrent record) 
		{
			data["roomId"] = record.Room.AdditionalInformation;
			data["dateFrom"] = record.From1.AddHours(-3).ToString("yyyy-MM-dd HH:mm");
			data["dateTo"] = record.To1.AddHours(-3).ToString("yyyy-MM-dd HH:mm");
			data["guests"] = "2";
			data["formUrl"] = "https://soulhelp.ru/portfolio/cab_rome/";

			JObject jObj = await SoulHelp_RequestPost("https://api.soulhelp.ru/v1/widget/order/create", appId, appSecret, token);
			
			string orderId = jObj["orderId"].ToString();
			if (orderId.IsNullOrEmpty())
				throw new ArgumentException("Не удалось забронировать!");
			string cost = jObj["cost"].ToString();
			data["costToCharge"] = cost;
			jObj = await SoulHelp_RequestPost($"https://api.soulhelp.ru/v1/widget/order/{orderId}/confirm", appId, appSecret, token);
			return orderId;
		}

		public static async Task<bool> SoulHelpCancel(string appId, string appSecret, string token, string ServiceRecord)
		{
            using var client = new HttpClient();

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://api.soulhelp.ru/v1/order/{ServiceRecord}/cancel");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Headers.Add("Accept", "application/json, text/plain, */*");
            request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
            request.Headers.Add("Booking-App-Id", appId);
            request.Headers.Add("Booking-App-Secret", appSecret);
            request.Headers.Referrer = new Uri("https://app.soulhelp.ru/");
            try
            {
                var response = await client.SendAsync(request);
                return JObject.Parse(await response.Content.ReadAsStringAsync()).ContainsKey("userBalance"); ;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
	}
}
