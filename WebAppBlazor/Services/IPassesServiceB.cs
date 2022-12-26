using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services
{
    public interface IPassesServiceB
    {
        public Task<PassToken> LoginAsync(PassToken pass);
    }
}
