using System.ComponentModel.DataAnnotations;

namespace WebAppBlazor.Data.Models
{
    public partial class Pass
    {
        public int? UserUirId { get; set; }
		[Required (ErrorMessage = "Необходимо указать логин")]
		public string UserLogin { get; set; } = null!;
		[Required (ErrorMessage = "Необходимо указать пароль")]
		public string PasswordHash { get; set; } = null!;

        public virtual UserTable? UserUir { get; set; }
    }

}