using Dapper;
using System.Data;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Dapper
{
    public class ScoreDA : BaseDA
    {
        #region Fields
        private const string TABLE_NAME = "public.\"tblScore\"";

        private IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public ScoreDA(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public Score Get(Guid id)
        {
            string query = $"SELECT * FROM {TABLE_NAME} where \"{nameof(Score.Id)}\" = @{nameof(Score.Id)};";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                return connection.QuerySingleOrDefault<Score>(query, new { Id = id });
            }
        }

        /*
            TODO:
            Paging needed here!
        */
        public IEnumerable<Score> GetByGameSeasonId(Guid gameSeasonId)
        {
            string query = $"SELECT * FROM {TABLE_NAME} WHERE \"{nameof(Score.GameSeasonId)}\" = @{nameof(Score.GameSeasonId)};";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                return connection.Query<Score>(query, new { GameSeasonId = gameSeasonId });
            }
        }

        public IResult Add(Score score)
        {
            string query = $"INSERT INTO {TABLE_NAME} (\"{nameof(Score.Id)}\", \"{nameof(Score.CreationDate)}\", \"{nameof(Score.GameSeasonId)}\", \"{nameof(Score.Point)}\") VALUES (@{nameof(Score.Id)}, @{nameof(Score.CreationDate)}, @{nameof(Score.GameSeasonId)}, @{nameof(Score.Point)});";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    connection.Query(query, new { Id = score.Id, CreationDate = score.CreationDate, GameSeasonId = score.GameSeasonId, Point = score.Point });

                    transaction.Commit();

                    return new Result<object>();

                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Console.WriteLine(exception);
                    return new ErrorResult<object>(new List<string> { "Can't add score", exception.ToString() });
                }
            }
        }
        #endregion
    }
}