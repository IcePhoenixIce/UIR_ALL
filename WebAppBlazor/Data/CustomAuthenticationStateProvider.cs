using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;
using System.Xml.Linq;
using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Data
{
	public class CustomAuthenticationStateProvider : AuthenticationStateProvider
	{
		private ILocalStorageService _localStorageService;

		public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
		{
			_localStorageService = localStorageService;
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
            ClaimsIdentity identity;
            var userJson = await _localStorageService.GetItemAsync<string>("user");
			if (userJson != null) 
			{ 
				var userFromStr = JsonConvert.DeserializeObject<UserTable>(userJson);
				
				if (userFromStr != null)
				{
					identity = new ClaimsIdentity(
						new[]
						{
						new Claim(ClaimTypes.Name, userFromStr.FirstName),
						}, "apiauth_type");
				}
				else
					identity = new ClaimsIdentity();
			}
			else
				identity = new ClaimsIdentity();
			var user = new ClaimsPrincipal(identity);

			return await Task.FromResult(new AuthenticationState(user));
		}

		public void MarkUserAsAuthenticated(UserTable userTable) 
		{
			string role = "Client";
			if (userTable.Specialist != null) role = "Specialist";
            var claims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, userTable.UserUirId.ToString()),
					new Claim(ClaimTypes.Name, userTable.LastName+" "+userTable.FirstName+" "+userTable.MiddleName),
					new Claim(ClaimTypes.Role, role)
				};
            var identity = new ClaimsIdentity(claims, "apiauth_type");
			var user = new ClaimsPrincipal(identity);

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}

		public void MarkUserAsLoggedOut()
		{
			_localStorageService.RemoveItemAsync("user");
			_localStorageService.RemoveItemAsync("tokenA");
			_localStorageService.RemoveItemAsync("tokenB");

			var identity = new ClaimsIdentity();
			var user = new ClaimsPrincipal(identity);

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}
	}
}
