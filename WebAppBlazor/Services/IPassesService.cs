using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services
{
    public interface IPassesService
    {
        public Task<PassToken> LoginAsync(PassToken pass);
        public Task<bool> SignUpAsync(UserTable User);
    }
}
