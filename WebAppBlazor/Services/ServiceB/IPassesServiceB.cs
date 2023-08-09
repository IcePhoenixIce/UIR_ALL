using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services.ServiceB
{
    public interface IPassesServiceB
    {
        public Task<PassToken> LoginAsync(PassToken pass);
    }
}
