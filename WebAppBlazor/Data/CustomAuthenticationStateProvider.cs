using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Principal;
using System.Xml.Linq;

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
			var userJson = await _localStorageService.GetItemAsync<string>("user");
			var userFromStr = JsonConvert.DeserializeObject<UserTable>(userJson);
			ClaimsIdentity identity;
			if (userFromStr != null)
			{
				identity = new ClaimsIdentity(
					new[]
					{
					new Claim(ClaimTypes.Name, userFromStr.FirstName),
					}, "apiauth_type");
			}
			else
			{
				identity = new ClaimsIdentity();
			}
			var user = new ClaimsPrincipal(identity);

			return await Task.FromResult(new AuthenticationState(user));
		}

		public void MarkUserAsAuthenticated(string name) 
		{
			var identity = new ClaimsIdentity(
				new[]
				{
					new Claim(ClaimTypes.Name, name),
				}, "apiauth_type");
			var user = new ClaimsPrincipal(identity);

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}

		public void MarkUserAsLoggedOut()
		{
			_localStorageService.RemoveItemAsync("user");
			_localStorageService.RemoveItemAsync("token");

			var identity = new ClaimsIdentity();
			var user = new ClaimsPrincipal(identity);

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}
	}
}
