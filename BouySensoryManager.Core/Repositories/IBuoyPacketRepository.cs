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
        /// Eject old records.
        /// </summary>
        Task<int> Eject(DateTime ejectOlderThan);

        /// <summary>
        /// Creates the table if it does not already exist.
        /// </summary>
        Task Initialize();
    }
}