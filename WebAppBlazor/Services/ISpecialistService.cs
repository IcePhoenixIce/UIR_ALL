using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services
{
    public interface ISpecialistService
	{
        public Task<IEnumerable<SpecialistName>?> SpecialistsAsync();
        public Task<Specialist?> SpecialistAsync(int id);
    }
}
