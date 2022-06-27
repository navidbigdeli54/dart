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
        [Test]
        public void AddLeaderboardTest()
        {
            Leaderboard leaderboard = new Leaderboard
            {
                Id = Guid.NewGuid(),
                GameSessionId = TestHelper.AddGameSession(_applicationContext, TestHelper.AddUser(_applicationContext)).Id,
                Rank = Random.Shared.Next(0, int.MaxValue),
                Score = Random.Shared.Next(0, int.MaxValue)
            };
            LeaderboardDA leaderboardDA = new LeaderboardDA(_applicationContext);
            IResult result = leaderboardDA.Add(leaderboard);
            Assert.That(result.IsSuccessful, Is.True);

            Leaderboard retrivedLeaderboard = leaderboardDA.Get(leaderboard.Id);
            Assert.That(leaderboard.Id, Is.EqualTo(retrivedLeaderboard.Id));
            Assert.That(leaderboard.GameSessionId, Is.EqualTo(retrivedLeaderboard.GameSessionId));
            Assert.That(leaderboard.Rank, Is.EqualTo(retrivedLeaderboard.Rank));
            Assert.That(leaderboard.Score, Is.EqualTo(retrivedLeaderboard.Score));

        }

        [Test]
        public void UpdateLeaderboardTest()
        {
            User user = TestHelper.AddUser(_applicationContext);
            GameSession gameSession = TestHelper.AddGameSession(_applicationContext, user);
            Leaderboard leaderboard = TestHelper.AddLeaderboard(_applicationContext, gameSession);

            leaderboard.Score = Random.Shared.Next(0, int.MaxValue);
            leaderboard.Rank = Random.Shared.Next(0, int.MaxValue);

            LeaderboardDA leaderboardDA = new LeaderboardDA(_applicationContext);
            IResult result = leaderboardDA.UpdateOrAdd(leaderboard);
            Assert.That(result.IsSuccessful, Is.True);

            Leaderboard retrivedLeaderboard = leaderboardDA.Get(leaderboard.Id);
            Assert.That(leaderboard.Id, Is.EqualTo(retrivedLeaderboard.Id));
            Assert.That(leaderboard.GameSessionId, Is.EqualTo(retrivedLeaderboard.GameSessionId));
            Assert.That(leaderboard.Rank, Is.EqualTo(retrivedLeaderboard.Rank));
            Assert.That(leaderboard.Score, Is.EqualTo(retrivedLeaderboard.Score));
        }

        [Test]
        public void GetAllTest()
        {
            TestHelper.ClearTable(_applicationContext, nameof(Leaderboard));

            TestHelper.AddLeaderboard(_applicationContext, TestHelper.AddGameSession(_applicationContext, TestHelper.AddUser(_applicationContext)));
            TestHelper.AddLeaderboard(_applicationContext, TestHelper.AddGameSession(_applicationContext, TestHelper.AddUser(_applicationContext)));
            TestHelper.AddLeaderboard(_applicationContext, TestHelper.AddGameSession(_applicationContext, TestHelper.AddUser(_applicationContext)));

            LeaderboardDA leaderboardDA = new LeaderboardDA(_applicationContext);
            Assert.That(3, Is.EqualTo(leaderboardDA.GetAll().Count()));
        }
        #endregion

        [Test]
        public void AddOrUpdateTest()
        {
            Leaderboard leaderboard = new Leaderboard
            {
                Id = Guid.NewGuid(),
                GameSessionId = TestHelper.AddGameSession(_applicationContext, TestHelper.AddUser(_applicationContext)).Id,
                Rank = Random.Shared.Next(0, int.MaxValue),
                Score = Random.Shared.Next(0, int.MaxValue)
            };
            LeaderboardDA leaderboardDA = new LeaderboardDA(_applicationContext);
            IResult result = leaderboardDA.UpdateOrAdd(leaderboard);
            Assert.That(result.IsSuccessful, Is.True);

            Leaderboard retrivedLeaderboard = leaderboardDA.Get(leaderboard.Id);
            Assert.That(leaderboard.Id, Is.EqualTo(retrivedLeaderboard.Id));
            Assert.That(leaderboard.GameSessionId, Is.EqualTo(retrivedLeaderboard.GameSessionId));
            Assert.That(leaderboard.Rank, Is.EqualTo(retrivedLeaderboard.Rank));
            Assert.That(leaderboard.Score, Is.EqualTo(retrivedLeaderboard.Score));

            leaderboard.Score = Random.Shared.Next(0, int.MaxValue);
            leaderboard.Rank = Random.Shared.Next(0, int.MaxValue);
            result = leaderboardDA.UpdateOrAdd(leaderboard);
            Assert.That(result.IsSuccessful, Is.True);

            retrivedLeaderboard = leaderboardDA.Get(leaderboard.Id);
            Assert.That(leaderboard.Id, Is.EqualTo(retrivedLeaderboard.Id));
            Assert.That(leaderboard.GameSessionId, Is.EqualTo(retrivedLeaderboard.GameSessionId));
            Assert.That(leaderboard.Rank, Is.EqualTo(retrivedLeaderboard.Rank));
            Assert.That(leaderboard.Score, Is.EqualTo(retrivedLeaderboard.Score));
        }
    }
}