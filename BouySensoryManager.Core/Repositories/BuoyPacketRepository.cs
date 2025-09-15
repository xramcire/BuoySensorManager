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

        public Task<int> Delete(DateTime cutOff)
        {
            throw new NotImplementedException();
        }

        public async Task Initialize()
        {
            try
            {
                await base.ExecuteAsync(createTable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing BuoyPackets datatable");
                throw;
            }
        }

        #region Queries

        const string create = @"
            INSERT INTO BuoyPackets (BuoyId, Depth, Amplitude, SeaLevel, CreatedOn)
            VALUES (@BuoyId, @Depth, @Amplitude, @SeaLevel, @CreatedOn)";

        const string createTable = @"
            CREATE TABLE IF NOT EXISTS BuoyPackets (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BuoyId TEXT NOT NULL,                    
                Depth REAL NOT NULL,
                Amplitude REAL NOT NULL,
                SeaLevel REAL NOT NULL,
                CreatedOn DATETIME NOT NULL
            )";

        #endregion
    }
}
