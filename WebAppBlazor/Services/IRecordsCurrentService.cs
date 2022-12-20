using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services
{
    public interface IRecordsCurrentService
    {
        public Task<IEnumerable<RecordCurrent>> UserAsync(int id);
    }
}
