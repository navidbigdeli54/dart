using Core.Dapper;
using NUnit.Framework;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.Dapper
{
    public class ScoreDATest
    {
        #region Fields
        private readonly IApplicationContext _applicationContext = new ApplicationContext();
        #endregion

        #region Public Methods
        [Test]
        public void AddScoreTest()
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

            Score score = new Score
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                GameSeasonId = gameSeason.Id,
                Point = Random.Shared.Next(0, int.MaxValue)
            };

            ScoreDA scoreDA = new ScoreDA(_applicationContext);
            result = scoreDA.Add(score);
            Assert.That(result.IsSuccessful, Is.True);

            Score retrivedScore = scoreDA.Get(score.Id);
            Assert.That(score.Id, Is.EqualTo(retrivedScore.Id));
            Assert.That(score.GameSeasonId, Is.EqualTo(retrivedScore.GameSeasonId));
            Assert.That(score.Point, Is.EqualTo(retrivedScore.Point));
        }

        [Test]
        public void AddMultipleScoreTest()
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

            Score score1 = new Score
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                GameSeasonId = gameSeason.Id,
                Point = Random.Shared.Next(0, int.MaxValue)
            };

            Score score2 = new Score
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                GameSeasonId = gameSeason.Id,
                Point = Random.Shared.Next(0, int.MaxValue)
            };

            ScoreDA scoreDA = new ScoreDA(_applicationContext);
            result = scoreDA.Add(score1);
            result = scoreDA.Add(score2);
            Assert.That(result.IsSuccessful, Is.True);

            Assert.That(2, Is.EqualTo(scoreDA.GetByGameSeasonId(gameSeason.Id).Count()));
        }
        #endregion
    }
}