using System.ComponentModel.DataAnnotations;

namespace WebAppBlazor.Data.Models
{
    public partial class Pass
    {
        public int? UserUirId { get; set; }
		[Required (ErrorMessage = "Необходимо указать логин")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Логин должен иметь длину от 4 до 20 символов")]
        public string UserLogin { get; set; } = null!;
		[Required (ErrorMessage = "Необходимо указать пароль")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен иметь длину от 6 до 20 символов")]
        public string PasswordHash { get; set; } = null!;

        public virtual UserTable? UserUir { get; set; }
    }

}