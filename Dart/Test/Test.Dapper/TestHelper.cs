using Dapper;
using Core.Dapper;
using System.Data;
using NUnit.Framework;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.Dapper
{
    public static class TestHelper
    {
        public static void ClearTable(IApplicationContext applicationContext, string model)
        {
            using (IDbConnection dbConnection = new Npgsql.NpgsqlConnection(applicationContext.DBConnectionString))
            {
                string query = $"DELETE FROM public.\"tbl{model}\";";

                dbConnection.Query(query);
            }
        }

        public static User AddUser(IApplicationContext applicationContext)
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Username = Guid.NewGuid().ToString(),
                EndPoint = "1.1.1.1:100"
            };

            UserDA userDA = new UserDA(applicationContext);
            IResult result = userDA.Add(user);
            Assert.That(result.IsSuccessful, Is.True);

            return user;
        }

        public static GameSeason AddGameSeason(IApplicationContext applicationContext, User user)
        {
            GameSeason gameSeason = new GameSeason
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CreationDate = DateTime.UtcNow
            };
            GameSeasonDA gameSeasonDA = new GameSeasonDA(applicationContext);
            IResult result = gameSeasonDA.Add(gameSeason);
            Assert.That(result.IsSuccessful, Is.True);

            return gameSeason;
        }

        public static Score AddScore(IApplicationContext applicationContext, GameSeason gameSeason)
        {
            Score score = new Score
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                GameSeasonId = gameSeason.Id,
                Point = Random.Shared.Next(0, int.MaxValue)
            };
            ScoreDA scoreDA = new ScoreDA(applicationContext);
            IResult result = scoreDA.Add(score);
            Assert.That(result.IsSuccessful, Is.True);

            return score;
        }

        public static Leaderboard AddLeaderboard(IApplicationContext applicationContext, GameSeason gameSeason)
        {
            Leaderboard leaderboard = new Leaderboard
            {
                Id = Guid.NewGuid(),
                GameSeasonId = gameSeason.Id,
                Rank = Random.Shared.Next(0, int.MaxValue),
                Score = Random.Shared.Next(0, int.MaxValue)
            };
            LeaderboardDA leaderboardDA = new LeaderboardDA(applicationContext);
            IResult result = leaderboardDA.Add(leaderboard);
            Assert.That(result.IsSuccessful, Is.True);

            return leaderboard;
        }
    }
}
