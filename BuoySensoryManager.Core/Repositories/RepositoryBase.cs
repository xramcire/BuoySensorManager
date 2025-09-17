using Dapper;
using System.Data;

namespace BuoySensorManager.Core.Repositories
{
    public class RepositoryBase
    {
        private readonly IDbConnection _connection;

        public RepositoryBase(IDbConnection connection)
        {
            _connection = connection;
        }

        protected Task<int> ExecuteAsync(string query)
            => _connection.ExecuteAsync(query);

        protected Task<int> ExecuteAsync(string query, object param)
            => _connection.ExecuteAsync(query, param);

        protected Task<IEnumerable<T>> QueryAsync<T>(string query, object param)
            => _connection.QueryAsync<T>(query, param);

        protected Task<object?> ExecuteScalarAsync(string query)
            => _connection.ExecuteScalarAsync(query);
    }
}
