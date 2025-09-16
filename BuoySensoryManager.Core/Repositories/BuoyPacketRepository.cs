using BuoySensorManager.Core.Models;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BuoySensorManager.Core.Repositories
{
    public class BuoyPacketRepository : RepositoryBase, IBuoyPacketRepository
    {
        private readonly ILogger<BuoyPacketRepository> _logger;

        public BuoyPacketRepository(
            IDbConnection connection,
            ILogger<BuoyPacketRepository> logger
        ) : base(connection)
        {
            _logger = logger;
        }

        public async Task<int> Create(BuoyPacket buoyPacket)
        {
            try
            {
                return await base.ExecuteAsync(create, buoyPacket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Creating BuoyPacket.");
                throw;
            }
        }

        public async Task<int> Eject(DateTime ejectOlderThan)
        {
            try
            {
                var param = new { ReadingOn = ejectOlderThan.Ticks };
                return await base.ExecuteAsync(eject, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecting BuoyPackets.");
                throw;
            }
        }

        public async Task Initialize()
        {
            try
            {
                await base.ExecuteAsync(createTable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing BuoyPackets datatable.");
                throw;
            }
        }

        #region Queries

        const string create = @"
            INSERT INTO BuoyPackets (Port, Depth, Amplitude, SeaLevel, ReadingOn)
            VALUES (@Port, @Depth, @Amplitude, @SeaLevel, @ReadingOn)";

        const string createTable = @"
            CREATE TABLE IF NOT EXISTS BuoyPackets (
                Port INTEGER NOT NULL,
                Depth REAL NOT NULL,
                Amplitude REAL NOT NULL,
                SeaLevel REAL NOT NULL,
                ReadingOn INTEGER NOT NULL
            )";

        const string eject = @"DELETE FROM BuoyPackets WHERE ReadingOn < @ReadingOn";

        #endregion
    }
}
