using Newtonsoft.Json;
using System.Text;
using WebAppBlazor.Data.Models;
using System.Security.Cryptography;

namespace WebAppBlazor.Services.ServiceB
{
    public class PassesServiceB : IPassesServiceB
    {
        public HttpClient _httpClient { get; }

        public PassesServiceB(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private string hashPassword(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += string.Format("{0:x2}", x);
            }
            return hashString;
        }

        public async Task<PassToken> LoginAsync(PassToken pass)
        {
            //pass.PasswordHash = hashPassword(pass.PasswordHash);

            string serializePassToken = JsonConvert.SerializeObject(pass);

            var requstMassage = new HttpRequestMessage(HttpMethod.Post, "Passes/Login");
            requstMassage.Content = new StringContent(serializePassToken);
            requstMassage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json-patch+json");

            var response = await _httpClient.SendAsync(requstMassage);
            var responseStatusCode = response.StatusCode;
            if (responseStatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var returned_user = JsonConvert.DeserializeObject<PassToken>(responseBody);
                return await Task.FromResult(returned_user);
            }
            return await Task.FromResult(new PassToken());
        }
    }
}
