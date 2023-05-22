using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services
{
    public interface IRoomService
	{
        public Task<ICollection<Room>?> RoomsAsync(int areaId);
        public Task<(Room, IDictionary<DateTime, IEnumerable<RecordService>>)?> RoomAsync(int id);
    }
}
