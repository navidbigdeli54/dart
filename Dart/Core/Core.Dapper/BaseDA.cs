using Npgsql;
using System.Data;

namespace Core.Dapper
{
    public abstract class BaseDA
    {
        #region  Protected Methods
        protected static IDbConnection OpenConnection(string connectionString)
        {
            IDbConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();
            return connection;
        }
        #endregion
    }
}