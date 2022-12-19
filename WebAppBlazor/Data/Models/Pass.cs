namespace WebAppBlazor.Data.Models
{
    public partial class Pass
    {
        public int? UserUirId { get; set; }

        public string UserLogin { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public virtual UserTable? UserUir { get; set; }
    }

}