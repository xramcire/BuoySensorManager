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

        public async ValueTask<int> Count()
        {
            try
            {
                var result = await base.ExecuteScalarAsync(count);
                return result is null ? 0 : Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Counting BuoyPackets.");
                throw;
            }
        }

        public async ValueTask<int> Create(BuoyPacket buoyPacket)
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

        public async ValueTask<int> Delete(Guid id)
        {
            try
            {
                var param = new { Id = id };
                return await base.ExecuteAsync(delete, param);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Deleting BuoyPacket.");
                throw;
            }
        }

        public async ValueTask Initialize()
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

        public async ValueTask<IReadOnlyList<BuoyPacket>> Fetch(int limit)
        {
            try
            {
                var param = new { Limit = limit };
                var result = await base.QueryAsync<BuoyPacket>(list, param);
                return [.. result];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing unsent BuoyPackets.");
                throw;
            }
        }

        #region Queries
        //
        //  I've chosen old fashioned string based queries here because they are the most efficient method.
        //  Any type of query generation has overhead. Given the time it takes to write these "scripts" vs
        //  overhead wasted generating queries repeatedly. I choose this. In another context I might choose
        //  differently.
        //
        const string count = "SELECT COUNT(1) FROM BuoyPackets";

        const string create = @"
            INSERT INTO BuoyPackets (Id, Port, Depth, SeaLevel, ReadingOn)
            VALUES (@Id, @Port, @Depth, @SeaLevel, @ReadingOn)";

        const string createTable = @"
            CREATE TABLE IF NOT EXISTS BuoyPackets (
                Id UNIQUEIDENTIFIER NOT NULL,
                Port INTEGER NOT NULL,
                Depth REAL NOT NULL,
                SeaLevel REAL NULL,
                ReadingOn INTEGER NOT NULL
            )";

        const string delete = "DELETE FROM BuoyPackets WHERE Id = @Id";

        const string list = "SELECT * FROM BuoyPackets ORDER BY ReadingOn DESC LIMIT @Limit";

        #endregion
    }
}
