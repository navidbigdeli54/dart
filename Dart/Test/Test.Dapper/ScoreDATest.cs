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
            Score score = new Score
            {
                Id = Guid.NewGuid(),
                CreationDate = DateTime.UtcNow,
                GameSeasonId = TestHelper.AddGameSeason(_applicationContext, TestHelper.AddUser(_applicationContext)).Id,
                Point = Random.Shared.Next(0, int.MaxValue)
            };
            ScoreDA scoreDA = new ScoreDA(_applicationContext);
            IResult result = scoreDA.Add(score);
            Assert.That(result.IsSuccessful, Is.True);

            Score retrivedScore = scoreDA.Get(score.Id);
            Assert.That(score.Id, Is.EqualTo(retrivedScore.Id));
            Assert.That(score.GameSeasonId, Is.EqualTo(retrivedScore.GameSeasonId));
            Assert.That(score.Point, Is.EqualTo(retrivedScore.Point));
        }

        [Test]
        public void AddMultipleScoreTest()
        {
            User user = TestHelper.AddUser(_applicationContext);
            GameSeason gameSeason = TestHelper.AddGameSeason(_applicationContext, user);
            Score score1 = TestHelper.AddScore(_applicationContext, gameSeason);
            Score score2 = TestHelper.AddScore(_applicationContext, gameSeason);

            ScoreDA scoreDA = new ScoreDA(_applicationContext);
            Assert.That(2, Is.EqualTo(scoreDA.GetByGameSeasonId(gameSeason.Id).Count()));
        }
        #endregion
    }
}