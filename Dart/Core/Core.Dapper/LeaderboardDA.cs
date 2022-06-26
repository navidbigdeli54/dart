using Dapper;
using System.Data;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Dapper
{
    public class LeaderboardDA : BaseDA
    {
        #region Fields
        private const string TABLE_NAME = "public.\"tblLeaderboard\"";

        private IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public LeaderboardDA(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public Leaderboard Get(Guid id)
        {
            string query = $"SELECT * FROM {TABLE_NAME} where \"{nameof(Leaderboard.Id)}\" = @{nameof(Leaderboard.Id)};";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                return connection.QuerySingleOrDefault<Leaderboard>(query, new { Id = id });
            }
        }

        /*
            TODO:
            Paging needed here!
        */
        public IEnumerable<Leaderboard> GetAll()
        {
            string query = $"SELECT * FROM {TABLE_NAME};";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                return connection.Query<Leaderboard>(query);
            }
        }

        public IResult Add(Leaderboard leaderboard)
        {
            string query = $"INSERT INTO {TABLE_NAME} (\"{nameof(Leaderboard.Id)}\", \"{nameof(Leaderboard.GameSeasonId)}\", \"{nameof(Leaderboard.Score)}\", \"{nameof(Leaderboard.Rank)}\") VALUES (@{nameof(Leaderboard.Id)}, @{nameof(Leaderboard.GameSeasonId)}, @{nameof(Leaderboard.Score)}, @{nameof(Leaderboard.Rank)});";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    connection.Query(query, new { Id = leaderboard.Id, GameSeasonId = leaderboard.GameSeasonId, Score = leaderboard.Score, Rank = leaderboard.Rank });

                    transaction.Commit();

                    return new Result<object>();

                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Console.WriteLine(exception);
                    return new ErrorResult<object>(new List<string> { "Can't add leaderboard", exception.ToString() });
                }
            }
        }

        public IResult Update(Leaderboard leaderboard)
        {
            string query = $"UPDATE {TABLE_NAME} SET \"{nameof(Leaderboard.Score)}\" = @{nameof(Leaderboard.Score)}, \"{nameof(Leaderboard.Rank)}\" = @{nameof(Leaderboard.Rank)} WHERE \"{nameof(Leaderboard.Id)}\" = @{nameof(Leaderboard.Id)};";

            using (IDbConnection connection = OpenConnection(_applicationContext.DBConnectionString))
            {
                IDbTransaction transaction = connection.BeginTransaction();
                try
                {
                    connection.Query(query, new { Id = leaderboard.Id, Score = leaderboard.Score, Rank = leaderboard.Rank });

                    transaction.Commit();

                    return new Result<object>();

                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    Console.WriteLine(exception);
                    return new ErrorResult<object>(new List<string> { "Can't update leaderboard", exception.ToString() });
                }
            }
        }
        #endregion
    }
}