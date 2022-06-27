using Core.BL;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.BL
{
    public class GameSessionTest
    {
        [Test]
        public void AddGameSessionTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, string.Empty);

            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);
            IResult<Guid> result = gameSessionBL.Add(user.Id);
            Assert.That(result.IsSuccessful, Is.True);

            ImmutableGameSession gameSession = gameSessionBL.Get(result.Message);
            Assert.Multiple(() =>
            {
                Assert.That(gameSession.Id, Is.EqualTo(result.Message));
                Assert.That(gameSession.UserId, Is.EqualTo(user.Id));
                Assert.That(gameSession.Scores.Select(x => x.Point).Sum(), Is.EqualTo(0));
            });
        }

        [Test]
        public void AddGameSessionWittInvalidUserTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);
            IResult<Guid> result = gameSessionBL.Add(Guid.Empty);
            Assert.That(result.IsSuccessful, Is.False);
        }

        [Test]
        public void AddGameSessionForAUserTwiceTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, string.Empty);
            ImmutableGameSession gameSession = TestHelper.AddGameSession(applicationContext, user);

            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);
            IResult<Guid> result = gameSessionBL.Add(user.Id);
            Assert.That(result.IsSuccessful, Is.False);
        }

        [Test]
        public void GetByUserIdTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, string.Empty);
            ImmutableGameSession gameSession = TestHelper.AddGameSession(applicationContext, user);

            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);
            ImmutableGameSession retrivedUGameSession = gameSessionBL.GetByUserId(user.Id);
            Assert.Multiple(() =>
            {
                Assert.That(retrivedUGameSession.Id, Is.EqualTo(gameSession.Id));
                Assert.That(retrivedUGameSession.UserId, Is.EqualTo(gameSession.UserId));
                Assert.That(retrivedUGameSession.Scores.Select(x => x.Point).Sum(), Is.EqualTo(gameSession.Scores.Select(x => x.Point).Sum()));
            });
        }

        [Test]
        public void AddScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();
            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession = TestHelper.AddGameSession(applicationContext, user);

            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);
            int point = Random.Shared.Next(0, int.MaxValue);
            IResult result = gameSessionBL.AddNewScore(user.Id, point);
            Assert.That(result.IsSuccessful, Is.True);

            ImmutableGameSession retrivedGameSession = gameSessionBL.Get(gameSession.Id);
            Assert.Multiple(() =>
            {
                Assert.That(retrivedGameSession.Id, Is.EqualTo(gameSession.Id));
                Assert.That(retrivedGameSession.Scores.Select(x => x.Point).Sum(), Is.EqualTo(point));
            });
        }

        [Test]
        public void AddMultipleScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();
            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession = TestHelper.AddGameSession(applicationContext, user);

            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);

            int scoreSum = 0;
            for (int i = 0; i < ImmutableGameSession.MAX_SCORE_NUMBER; ++i)
            {
                int point = Random.Shared.Next(0, int.MaxValue / ImmutableGameSession.MAX_SCORE_NUMBER);
                IResult result = gameSessionBL.AddNewScore(user.Id, point);
                Assert.That(result.IsSuccessful, Is.True);
                scoreSum +=point;
            }

            ImmutableGameSession retrivedGameSession = gameSessionBL.Get(gameSession.Id);
            Assert.Multiple(() =>
            {
                Assert.That(retrivedGameSession.Id, Is.EqualTo(gameSession.Id));
                Assert.That(retrivedGameSession.Scores.Select(x => x.Point).Sum(), Is.EqualTo(scoreSum));
            });
        }

        [Test]
        public void AddMoreThanAllowedScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();
            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession = TestHelper.AddGameSession(applicationContext, user);

            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);

            for (int i = 0; i < ImmutableGameSession.MAX_SCORE_NUMBER; ++i)
            {
                int point = Random.Shared.Next(0, int.MaxValue / ImmutableGameSession.MAX_SCORE_NUMBER);
                IResult result = gameSessionBL.AddNewScore(user.Id, point);
                Assert.That(result.IsSuccessful, Is.True);
            }

            IResult error = gameSessionBL.AddNewScore(user.Id, Random.Shared.Next(0, int.MaxValue));
            Assert.That(error.IsSuccessful, Is.False);
        }

        [Test]
        public void AddScoreAfterMaxAllowedTimeTest()
        {
            IApplicationContext applicationContext = new ApplicationContext();
            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");

            GameSession gameSession = new GameSession
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CreationDate = DateTime.UtcNow.AddMinutes(-10)
            };

            applicationContext.ApplicationCache.GameSession.Add(gameSession);

            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);
            IResult error = gameSessionBL.AddNewScore(user.Id, Random.Shared.Next(0, int.MaxValue));
            Assert.That(error.IsSuccessful, Is.False);
        }
    }
}