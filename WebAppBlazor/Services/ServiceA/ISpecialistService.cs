using Microsoft.AspNetCore.Mvc;
using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services.ServiceA
{
    public interface ISpecialistService
    {
        public Task<IEnumerable<Specialist>> SpecialistsAsync();
        public Task<Specialist> SpecialistAsync(int id);
        public Task<bool> SpecialistPutAsync(int id, Specialist spec);
        public Task<IEnumerable<RecordService>> GetAppointmentCurrentSpec(int id);
    }
}
