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

        protected int Execute(string query)
        {
            return _connection.Execute(query);
        }

        protected Task<int> ExecuteAsync(string query, object param)
        {
            return _connection.ExecuteAsync(query, param);
        }

        protected IDataReader ExecuteReader(string query)
        {
            return _connection.ExecuteReader(query);
        }

        protected object? ExecuteScalar(string query)
        {
            return _connection.ExecuteScalar(query);
        }
    }
}
