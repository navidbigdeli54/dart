using Dapper;
using System.Data;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Dapper
{
    public class GameSeasonDA : BaseDA
    {
        #region Fields
        private const string TABLE_NAME = "public.\"tblGameSeason\"";

        private IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public GameSeasonDA(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public GameSeason Get(int id)
        {
            string query = $"SELECT * FROM {TABLE_NAME} where \"{nameof(GameSeason.Id)}\" = @{nameof(GameSeason.Id)};";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                return connection.QuerySingleOrDefault<GameSeason>(query, new { Id = id });
            }
        }

        /*
            TODO:
            Paging needed here!
        */
        public GameSeason GetAll()
        {
            string query = $"SELECT * FROM {TABLE_NAME};";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                return connection.QuerySingleOrDefault<GameSeason>(query);
            }
        }

        public IResult Add(GameSeason gameSeason)
        {
            string query = $"INSERT INTO {TABLE_NAME} (\"{nameof(GameSeason.Id)}\", \"{nameof(GameSeason.CreationDate)}\", \"{nameof(GameSeason.UserId)}\") VALUES (@{nameof(GameSeason.Id)}, @{nameof(GameSeason.CreationDate)}, @{nameof(GameSeason.UserId)});";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    connection.Query(query, new { Id = gameSeason.Id, CreationDate = gameSeason.CreationDate, UserId = gameSeason.UserId });

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