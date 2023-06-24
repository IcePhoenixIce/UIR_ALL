using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services
{
    public interface IRecordsCurrentService
    {
        public Task<IEnumerable<RecordCurrent>> UserAsync(int id);
        public Task<IEnumerable<RecordCurrent>> SpecAsync(int id);
        public Task<RecordCurrent?> PostRecordAsync(RecordCurrent record);
        public Task<bool> DeleteAsync(int id);
    }
}
