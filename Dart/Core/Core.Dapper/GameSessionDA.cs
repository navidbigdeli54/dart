using Dapper;
using System.Data;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Dapper
{
    public class GameSessionDA : BaseDA
    {
        #region Fields
        private const string TABLE_NAME = "public.\"tblGameSession\"";

        private IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public GameSessionDA(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public GameSession Get(Guid id)
        {
            string query = $"SELECT * FROM {TABLE_NAME} where \"{nameof(GameSession.Id)}\" = @{nameof(GameSession.Id)};";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                return connection.QuerySingleOrDefault<GameSession>(query, new { Id = id });
            }
        }

        /*
            TODO:
            Paging needed here!
        */
        public IReadOnlyList<GameSession> GetAll()
        {
            string query = $"SELECT * FROM {TABLE_NAME};";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                return connection.Query<GameSession>(query).ToList();
            }
        }

        public IResult Add(GameSession gameSession)
        {
            string query = $"INSERT INTO {TABLE_NAME} (\"{nameof(GameSession.Id)}\", \"{nameof(GameSession.CreationDate)}\", \"{nameof(GameSession.UserId)}\") VALUES (@{nameof(GameSession.Id)}, @{nameof(GameSession.CreationDate)}, @{nameof(GameSession.UserId)});";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    connection.Query(query, new { Id = gameSession.Id, CreationDate = gameSession.CreationDate, UserId = gameSession.UserId }, transaction);

                    transaction.Commit();

                    return new Result<object>();

                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Console.WriteLine(exception);
                    return new ErrorResult<object>(new List<string> { "Can't add game season", exception.ToString() });
                }
            }
        }
        #endregion
    }
}