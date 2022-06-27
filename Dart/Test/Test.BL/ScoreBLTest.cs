using Core.BL;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.BL
{
    public class ScoreBLTest
    {
        [Test]
        public void AddScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession = TestHelper.AddGameSession(applicationContext, user);

            ScoreBL scoreBL = new ScoreBL(applicationContext);
            int randomPoint = Random.Shared.Next(0, int.MaxValue);
            IResult<Guid> result = scoreBL.Add(gameSession.Id, randomPoint);
            Assert.That(result.IsSuccessful, Is.True);

            ImmutableScore score = scoreBL.Get(result.Message);
            Assert.Multiple(() =>
            {
                Assert.That(score.Id, Is.EqualTo(result.Message));
                Assert.That(score.Point, Is.EqualTo(randomPoint));
                Assert.That(score.GameSessionId, Is.EqualTo(gameSession.Id));
            });
        }

        [Test]
        public void AddScoreByInvalidGameSessionTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ScoreBL scoreBL = new ScoreBL(applicationContext);
            int randomPoint = Random.Shared.Next(0, int.MaxValue);
            IResult<Guid> result = scoreBL.Add(Guid.NewGuid(), randomPoint);
            Assert.That(result.IsSuccessful, Is.False);
        }

        [Test]
        public void GetByGameSessionIdTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user1 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession1 = TestHelper.AddGameSession(applicationContext, user1);

            ImmutableScore score1 = TestHelper.AddScore(applicationContext, gameSession1, Random.Shared.Next(0, int.MaxValue));
            ImmutableScore score2 = TestHelper.AddScore(applicationContext, gameSession1, Random.Shared.Next(0, int.MaxValue));
            ImmutableScore score3 = TestHelper.AddScore(applicationContext, gameSession1, Random.Shared.Next(0, int.MaxValue));

            ImmutableUser user2 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession2 = TestHelper.AddGameSession(applicationContext, user2);

            ImmutableScore score4 = TestHelper.AddScore(applicationContext, gameSession2, Random.Shared.Next(0, int.MaxValue));

            ScoreBL scoreBL = new ScoreBL(applicationContext);

            IReadOnlyList<ImmutableScore> user1Scores = scoreBL.GetByGameSessionId(gameSession1.Id);
            Assert.Multiple(() =>
            {
                Assert.That(user1Scores, Has.Count.EqualTo(3));
                Assert.That(user1Scores.Any(x => x.Id == score1.Id), Is.True);
                Assert.That(user1Scores.Any(x => x.Id == score2.Id), Is.True);
                Assert.That(user1Scores.Any(x => x.Id == score3.Id), Is.True);
            });

            IReadOnlyList<ImmutableScore> user2Scores = scoreBL.GetByGameSessionId(gameSession2.Id);
            Assert.Multiple(() =>
            {
                Assert.That(user2Scores.Count, Is.EqualTo(1));
                Assert.That(user2Scores.Any(x => x.Id == score4.Id), Is.True);
            });
        }
    }
}