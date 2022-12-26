using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services
{
    public interface IAreaService
	{
        public Task<IEnumerable<Area>?> AreasAsync();
        public Task<Area?> AreaAsync(int id);
    }
}
