namespace WebAppBlazor.Data.Models
{
    public partial class PassToken : Pass
	{
		public PassToken(Pass pass)
		{
			this.UserUirId = pass.UserUirId;
			this.UserLogin = pass.UserLogin;
			this.PasswordHash = pass.PasswordHash;
			this.UserUir = pass.UserUir;
		}
		public PassToken() { }
		public string Token { get; set; }
	}
}

