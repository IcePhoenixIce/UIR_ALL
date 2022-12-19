using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
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
			var name = await _localStorageService.GetItemAsync<string>("name");
			ClaimsIdentity identity;
			if (name != null)
			{
				identity = new ClaimsIdentity(
					new[]
					{
					new Claim(ClaimTypes.Name, name),
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
			_localStorageService.RemoveItemAsync("name");
			_localStorageService.RemoveItemAsync("token");

			var identity = new ClaimsIdentity();
			var user = new ClaimsPrincipal(identity);

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
		}
	}
}
