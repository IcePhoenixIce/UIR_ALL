using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services
{
    public interface IAppointmentCurrentService
	{
        public Task<IEnumerable<AppointmentCurrent>?> UserAsync(int id);
        public Task<IEnumerable<AppointmentCurrent>?> SpecialistAsync(int id);
    }
}
