using BuoySensorManager.Core.Models;

namespace BuoySensorManager.Core.Repositories
{
    public interface IBuoyPacketRepository
    {
        /// <summary>
        /// Creates a new record.
        /// </summary>
        Task<int> Create(BuoyPacket buoyPacket);

        /// <summary>
        /// Deletes anything older than the cutOff date.
        /// </summary>
        Task<int> Delete(DateTime cutOff);

        /// <summary>
        /// Creates the table if it does not already exist.
        /// </summary>
        Task Initialize();
    }
}