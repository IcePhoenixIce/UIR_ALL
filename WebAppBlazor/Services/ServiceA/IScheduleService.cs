using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services.ServiceA
{
    public interface IScheduleService
    {
        public Task<bool> ShedulePutAsync(int id, SheduleTable shedule);
        public Task<bool> ShedulePostAsync(SheduleTable shedule);
        public Task<bool> SheduleDeleteAsync(int SpecId, int WeekId);
    }
}
