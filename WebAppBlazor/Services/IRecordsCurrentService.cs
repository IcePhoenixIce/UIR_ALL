using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services
{
    public interface IRecordsCurrentService
    {
        public Task<IEnumerable<RecordCurrent>> UserAsync(int id);
        public Task<RecordCurrent?> PostRecordAsync(RecordCurrent record, int specialistID, decimal price);
        public Task<bool> DeleteAsync(int id);
    }
}
