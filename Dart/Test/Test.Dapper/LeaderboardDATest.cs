using Core.Dapper;
using NUnit.Framework;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.Dapper
{
    public class LeaderboardDATest
    {
        #region Fields
        private readonly IApplicationContext _applicationContext = new ApplicationContext();
        #endregion

        #region Public Methods
        public void AddLeaderboardTest()
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Username = Guid.NewGuid().ToString(),
                EndPoint = "1.1.1.1:100"
            };
            UserDA userDA = new UserDA(_applicationContext);
            IResult result = userDA.Add(user);
            Assert.That(result.IsSuccessful, Is.True);

            GameSeason gameSeason = new GameSeason
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CreationDate = DateTime.UtcNow
            };
            GameSeasonDA gameSeasonDA = new GameSeasonDA(_applicationContext);
            result = gameSeasonDA.Add(gameSeason);
            Assert.That(result.IsSuccessful, Is.True);

            Leaderboard leaderboard = new Leaderboard
            {
                Id = Guid.NewGuid(),
                GameSeasonId = gameSeason.Id,
                Rank = Random.Shared.Next(0, int.MaxValue),
                Score = Random.Shared.Next(0, int.MaxValue)
            };

            LeaderboardDA leaderboardDA = new LeaderboardDA(_applicationContext);
            result = leaderboardDA.Add(leaderboard);
            Assert.That(result.IsSuccessful, Is.True);

            Leaderboard retrivedLeaderboard = leaderboardDA.Get(leaderboard.Id);
            Assert.That(leaderboard.Id, Is.EqualTo(retrivedLeaderboard.Id));
            Assert.That(leaderboard.GameSeasonId, Is.EqualTo(retrivedLeaderboard.GameSeasonId));
            Assert.That(leaderboard.Rank, Is.EqualTo(retrivedLeaderboard.Rank));
            Assert.That(leaderboard.Score, Is.EqualTo(retrivedLeaderboard.Score));

        }
        
        public void UpdateLeaderboardTest()
        {
            User user = new User
            {
                Id = Guid.NewGuid(),
                Username = Guid.NewGuid().ToString(),
                EndPoint = "1.1.1.1:100"
            };
            UserDA userDA = new UserDA(_applicationContext);
            IResult result = userDA.Add(user);
            Assert.That(result.IsSuccessful, Is.True);

            GameSeason gameSeason = new GameSeason
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CreationDate = DateTime.UtcNow
            };
            GameSeasonDA gameSeasonDA = new GameSeasonDA(_applicationContext);
            result = gameSeasonDA.Add(gameSeason);
            Assert.That(result.IsSuccessful, Is.True);

            Leaderboard leaderboard = new Leaderboard
            {
                Id = Guid.NewGuid(),
                GameSeasonId = gameSeason.Id,
                Rank = Random.Shared.Next(0, int.MaxValue),
                Score = Random.Shared.Next(0, int.MaxValue)
            };

            LeaderboardDA leaderboardDA = new LeaderboardDA(_applicationContext);
            result = leaderboardDA.Add(leaderboard);
            Assert.That(result.IsSuccessful, Is.True);

            leaderboard.Score = Random.Shared.Next(0, int.MaxValue);
            leaderboard.Rank = Random.Shared.Next(0, int.MaxValue);
            result = leaderboardDA.Update(leaderboard);
            Assert.That(result.IsSuccessful, Is.True);

            Leaderboard retrivedLeaderboard = leaderboardDA.Get(leaderboard.Id);
            Assert.That(leaderboard.Id, Is.EqualTo(retrivedLeaderboard.Id));
            Assert.That(leaderboard.GameSeasonId, Is.EqualTo(retrivedLeaderboard.GameSeasonId));
            Assert.That(leaderboard.Rank, Is.EqualTo(retrivedLeaderboard.Rank));
            Assert.That(leaderboard.Score, Is.EqualTo(retrivedLeaderboard.Score));

        }
        #endregion
    }
}