using BuoySensorManager.Core.Models;

namespace BuoySensorManager.Core.Repositories
{
    public interface IBuoyPacketRepository
    {
        /// <summary>
        /// Counts of all records.
        /// </summary>
        ValueTask<long> Count();

        /// <summary>
        /// Creates a new record.
        /// </summary>
        ValueTask<int> Create(BuoyPacket buoyPacket);

        /// <summary>
        /// Delete record.
        /// </summary>
        ValueTask<int> Delete(Guid id);

        /// <summary>
        /// Fetches the given number of records in revsere chronological order.
        /// </summary>
        ValueTask<IReadOnlyList<BuoyPacket>> Fetch(int limit);

        /// <summary>
        /// Creates the table if it does not already exist.
        /// </summary>
        ValueTask Initialize();
    }
}