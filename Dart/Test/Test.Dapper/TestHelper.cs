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

        public static GameSession AddGameSession(IApplicationContext applicationContext, User user)
        {
            GameSession gameSession = new GameSession
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CreationDate = DateTime.UtcNow
            };
            GameSessionDA gameSessionDA = new GameSessionDA(applicationContext);
            IResult result = gameSessionDA.Add(gameSession);
            Assert.That(result.IsSuccessful, Is.True);

            return gameSession;
        }

        public static Score AddScore(IApplicationContext applicationContext, GameSession gameSession)
        {
            Score score = new Score
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                GameSessionId = gameSession.Id,
                Point = Random.Shared.Next(0, int.MaxValue)
            };
            ScoreDA scoreDA = new ScoreDA(applicationContext);
            IResult result = scoreDA.Add(score);
            Assert.That(result.IsSuccessful, Is.True);

            return score;
        }

        public static Leaderboard AddLeaderboard(IApplicationContext applicationContext, GameSession gameSession)
        {
            Leaderboard leaderboard = new Leaderboard
            {
                Id = Guid.NewGuid(),
                GameSessionId = gameSession.Id,
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
