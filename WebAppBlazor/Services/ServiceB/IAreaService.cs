using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services.ServiceB
{
    public interface IAreaService
    {
        public Task<IEnumerable<Area>?> AreasAsync();
        public Task<Area?> AreaAsync(int id);
    }
}
