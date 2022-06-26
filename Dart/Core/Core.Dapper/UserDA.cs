using Dapper;
using System.Data;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Dapper
{
    public class UserDA : BaseDA
    {
        #region Fields
        private const string TABLE_NAME = "public.\"tblUser\"";

        private IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public UserDA(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public User Get(Guid id)
        {
            string query = $"SELECT * FROM {TABLE_NAME} where \"{nameof(User.Id)}\" = @{nameof(User.Id)};";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                return connection.QuerySingleOrDefault<User>(query, new { Id = id });
            }
        }

        /*
            TODO:
            Paging needed here!
        */
        public IReadOnlyList<User> GetAll()
        {
            string query = $"SELECT * FROM {TABLE_NAME};";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                return connection.Query<User>(query).ToList();
            }
        }

        public IResult Add(User user)
        {
            string query = $"INSERT INTO {TABLE_NAME} (\"{nameof(User.Id)}\", \"{nameof(User.Username)}\", \"{nameof(User.EndPoint)}\") VALUES (@{nameof(User.Id)}, @{nameof(User.Username)}, @{nameof(User.EndPoint)});";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    connection.Query(query, new { Id = user.Id, Username = user.Username, EndPoint = user.EndPoint }, transaction);

                    transaction.Commit();

                    return new Result<object>();

                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Console.WriteLine(exception);
                    return new ErrorResult<object>(new List<string> { "Can't add User", exception.ToString() });
                }
            }
        }
        #endregion
    }
}