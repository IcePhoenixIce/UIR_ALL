using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Services
{
    public interface IRoomService
	{
        public Task<IEnumerable<Room>?> RoomsAsync(int areaId);
        public Task<Room?> RoomAsync(int id);
    }
}
