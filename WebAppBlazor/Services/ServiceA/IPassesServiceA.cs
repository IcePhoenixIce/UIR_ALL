using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services.ServiceA
{
    public interface IPassesServiceA
    {
        public Task<PassToken> LoginAsync(PassToken pass);
        public Task<bool> SignUpAsync(UserTable User);
    }
}
