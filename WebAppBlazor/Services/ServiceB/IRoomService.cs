using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services.ServiceB
{
    public interface IRoomService
    {
        public Task<ICollection<Room>?> RoomsAsync(int areaId);
        public Task<IEnumerable<RecordService>?> RoomAsync(int id);
    }
}
