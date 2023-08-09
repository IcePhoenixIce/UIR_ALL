using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using UIR_Service_B.Models;

namespace UIR_Service_B.ServiceBooking
{
	
	public class ASHRequests
	{
        static Dictionary<string, string> data = new Dictionary<string, string>();
        static JObject json;
		const int days = 7;

		static readonly IFormatProvider _ifp = CultureInfo.InvariantCulture;
		private static async Task<JObject> POSTrequestASH(Dictionary<string, string> data)
		{
			using var client = new HttpClient();
			var content = new FormUrlEncodedContent(data);

			var contentString = await content.ReadAsStringAsync();
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://a-n-h.space/wp-admin/admin-ajax.php"),
			};
			request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/111.0");
			request.Headers.Add("Accept", "*/*");
			request.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
			request.Headers.Referrer = new Uri("https://a-n-h.space/raspisanie");
			request.Content = content;
			var response = await client.SendAsync(request);
			return JObject.Parse(await response.Content.ReadAsStringAsync());
		}

		public static async Task<string> ASHToken(PassGarmony pass) 
		{
			data["email"] = pass.Login;
			data["pass"] = pass.Password;
			data["action"] = "login";
			json = await POSTrequestASH(data);
			string token = json["result"].ToString();
			data.Clear();
			return token;
		}

		public static async Task<(decimal, decimal)> ASHBalance(string token)
		{
			data["token"] = token;
			data["action"] = "balance";
			json = await POSTrequestASH(data);
			decimal balance = Convert.ToDecimal(json["result"]?["balance"], _ifp);
			decimal sale = Convert.ToDecimal(json["result"]?["sale"], _ifp);
			data.Clear();
			return (balance, sale);
		}

		public static async Task<string> ASHMe(string token)
		{
			data["token"] = token;
			data["action"] = "me";
			json = await POSTrequestASH(data);
			string userID = Convert.ToString(json["result"]?["id"], _ifp);
			data.Clear();
			return userID;
		}

		public static async Task<string> ASHLock(string token, string userID, RecordCurrent recordCurrent, DateTime from)
		{
			data["cabinet"] = recordCurrent.RoomId.ToString();
			data["user_id"] = userID;
			data["token"] = token;
			data["action"] = "lock";
			data["date"] = from.ToShortDateString();
			data["hour"] = from.Hour.ToString();
			data["minute"] = from.Minute == 0 ? "1/2" : "2/2";
			json = await POSTrequestASH(data);
			data.Clear();
			return json["result"]["id"].ToString();
		}

		public static async Task<bool> ASHdelete(string token, string userID, RecordCurrent recordCurrent)
		{
            data["id"] = recordCurrent.ServiceRecord;
            data["user_id"] = userID.ToString();
            data["token"] = token;
            data["action"] = "unlock";
            json = await POSTrequestASH(data);
            data.Clear();
			return Convert.ToBoolean(json["result"]);
            
        }

		public static async Task<IEnumerable<RecordService>> ASHRecordsServices(int id, int AreaId)
		{
			List<RecordService> recordsServices = new();
			Dictionary<string, string> data = new Dictionary<string, string>();
			DateTime date = DateTime.Now.Date;

			for (int i = 0; i < days; i++)
			{
				data["date"] = date.ToShortDateString();
				data["office"] = AreaId.ToString();
				data["action"] = "prices";
				data["token"] = "null";
				JObject jsonElement = await POSTrequestASH(data);
				var res = PricesServiceASH(JsonConvert.DeserializeObject<List<Price>>(jsonElement["result"]["prices"].ToString()), id, date);
				FindAllBookingForRoomASH(JsonConvert.DeserializeObject<List<booking>>(jsonElement["result"]["bookings"].ToString()), res, id);
				recordsServices.AddRange(res);
				date = date.AddDays(1);
			}
			return recordsServices;
		}

		private static List<RecordService> PricesServiceASH(List<Price> list_price, int roomID, DateTime date)
		{
			List<RecordService> records = new List<RecordService>();
			foreach (Price price in list_price)
			{
				if (price.CabinetId == roomID)
				{
					date = date.AddHours(price.Hour - date.Hour);
					date = date.AddMinutes(0 - date.Minute);
					records.Add(new RecordService { RoomID = roomID, From1 = date, To1 = date.AddMinutes(30), Price = price.price });
					records.Add(new RecordService { RoomID = roomID, From1 = date.AddMinutes(30), To1 = date.AddMinutes(60), Price = price.price });
				}
			}
			return records;
		}

		//Передать сюда уже массив для записей с ценами
		private static void FindAllBookingForRoomASH(List<booking> obj, List<RecordService> list_records, int roomID)
		{
			///МОДИФИКАЦИЯ: Отсортировать два массива и потом идти в два указателя, для совпадающих ячеек делать true; Сложность (O ( (k log k)+(n log n)+n+k)
			///Текущая сложность O(n*K)
			foreach (booking book in obj)
			{
				if (book.cabinet.id == list_records[0].RoomID)
					foreach (RecordService record in list_records)
						if (record.From1 == DateTime.ParseExact(book.beginAt, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))
						{
							record.IsBooked = true;
							break;
						}

			}
		}
	}
}
