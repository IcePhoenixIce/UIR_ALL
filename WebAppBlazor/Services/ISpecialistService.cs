using Microsoft.AspNetCore.Mvc;
using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services
{
    public interface ISpecialistService
	{
        public Task<IEnumerable<SpecialistName>?> SpecialistsAsync();
        public Task<SpecialistName?> SpecialistAsync(int id);

        public Task<ActionResult<(Specialist, IDictionary<DateTime, IEnumerable<RecordService>>)>> GetAppointmentCurrentSpec(int id);
    }
}
