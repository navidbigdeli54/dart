using Core.BL;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.BL
{
    public class LeaderboardBLTest
    {
        [Test]
        public void AddLeaderboardTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();
            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession = TestHelper.AddGameSession(applicationContext, user);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            IResult<Guid> result = leaderboadBL.Add(gameSession.Id);
            Assert.That(result.IsSuccessful, Is.True);

            ImmutableLeaderboard leaderboard = leaderboadBL.Get(result.Message);
            Assert.Multiple(() =>
            {
                Assert.That(leaderboard.Id, Is.EqualTo(result.Message));
                Assert.That(leaderboard.GameSessionId, Is.EqualTo(gameSession.Id));
                Assert.That(leaderboard.Rank, Is.EqualTo(1));
                Assert.That(leaderboard.Score, Is.EqualTo(0));
            });
        }

        [Test]
        public void AddLeaderboardTwiceForAGameSessionTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();
            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession = TestHelper.AddGameSession(applicationContext, user);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            IResult<Guid> result = leaderboadBL.Add(gameSession.Id);
            Assert.That(result.IsSuccessful, Is.True);

            ImmutableLeaderboard leaderboard = leaderboadBL.Get(result.Message);
            Assert.Multiple(() =>
            {
                Assert.That(leaderboard.Id, Is.EqualTo(result.Message));
                Assert.That(leaderboard.GameSessionId, Is.EqualTo(gameSession.Id));
                Assert.That(leaderboard.Rank, Is.EqualTo(1));
                Assert.That(leaderboard.Score, Is.EqualTo(0));
            });

            result = leaderboadBL.Add(gameSession.Id);
            Assert.That(result.IsSuccessful, Is.False);
        }

        [Test]
        public void AddLeaderboardWithoutGameSessionTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            IResult<Guid> result = leaderboadBL.Add(Guid.NewGuid());
            Assert.That(result.IsSuccessful, Is.False);
        }

        [Test]
        public void GetSomeEntriesTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user1 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession1 = TestHelper.AddGameSession(applicationContext, user1);
            ImmutableLeaderboard leaderboard1 = TestHelper.AddLeaderboard(applicationContext, gameSession1);

            ImmutableUser user2 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:101");
            ImmutableGameSession gameSession2 = TestHelper.AddGameSession(applicationContext, user2);
            ImmutableLeaderboard leaderboard2 = TestHelper.AddLeaderboard(applicationContext, gameSession2);

            ImmutableUser user3 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:102");
            ImmutableGameSession gameSession3 = TestHelper.AddGameSession(applicationContext, user3);
            ImmutableLeaderboard leaderboard3 = TestHelper.AddLeaderboard(applicationContext, gameSession3);

            ImmutableUser user4 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:102");
            ImmutableGameSession gameSession4 = TestHelper.AddGameSession(applicationContext, user4);
            ImmutableLeaderboard leaderboard4 = TestHelper.AddLeaderboard(applicationContext, gameSession4);


            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);
            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            IReadOnlyList<ImmutableLeaderboard> leaderBoardEntries = leaderboadBL.Get(3);
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));

                Assert.That(gameSessionBL.Get(leaderBoardEntries[0].GameSessionId).UserId, Is.EqualTo(user1.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[1].GameSessionId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[2].GameSessionId).UserId, Is.EqualTo(user3.Id));

                Assert.That(leaderBoardEntries[0].Rank, Is.EqualTo(1));
                Assert.That(leaderBoardEntries[1].Rank, Is.EqualTo(2));
                Assert.That(leaderBoardEntries[2].Rank, Is.EqualTo(3));
            });
        }

        [Test]
        public void AddScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();
            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession = TestHelper.AddGameSession(applicationContext, user);
            ImmutableLeaderboard leaderboard = TestHelper.AddLeaderboard(applicationContext, gameSession);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            int point = Random.Shared.Next(0, int.MaxValue);
            leaderboadBL.AddScore(user.Id, point);

            ImmutableLeaderboard retrivedGameSession = leaderboadBL.Get(leaderboard.Id);
            Assert.Multiple(() =>
            {
                Assert.That(retrivedGameSession.Id, Is.EqualTo(leaderboard.Id));
                Assert.That(retrivedGameSession.GameSessionId, Is.EqualTo(leaderboard.GameSessionId));
                Assert.That(retrivedGameSession.Rank, Is.EqualTo(1));
                Assert.That(retrivedGameSession.Score, Is.EqualTo(point));
            });
        }

        [Test]
        public void AddMoreThanAllowedScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();
            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession = TestHelper.AddGameSession(applicationContext, user);
            ImmutableLeaderboard leaderboard = TestHelper.AddLeaderboard(applicationContext, gameSession);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);

            for (int i = 0; i < ImmutableGameSession.MAX_SCORE_NUMBER; ++i)
            {
                int point = Random.Shared.Next(0, int.MaxValue / ImmutableGameSession.MAX_SCORE_NUMBER);
                IResult result = leaderboadBL.AddScore(user.Id, point);
                Assert.That(result.IsSuccessful, Is.True);
            }

            IResult error = leaderboadBL.AddScore(user.Id, Random.Shared.Next(0, int.MaxValue));
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

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            IResult error = leaderboadBL.AddScore(user.Id, Random.Shared.Next(0, int.MaxValue));
            Assert.That(error.IsSuccessful, Is.False);
        }

        [Test]
        public void AddEntriesThatHasSameScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user1 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession1 = TestHelper.AddGameSession(applicationContext, user1);
            ImmutableLeaderboard leaderboard1 = TestHelper.AddLeaderboard(applicationContext, gameSession1);

            ImmutableUser user2 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:101");
            ImmutableGameSession gameSession2 = TestHelper.AddGameSession(applicationContext, user2);
            ImmutableLeaderboard leaderboard2 = TestHelper.AddLeaderboard(applicationContext, gameSession2);

            ImmutableUser user3 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:102");
            ImmutableGameSession gameSession3 = TestHelper.AddGameSession(applicationContext, user3);
            ImmutableLeaderboard leaderboard3 = TestHelper.AddLeaderboard(applicationContext, gameSession3);


            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);
            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            IReadOnlyList<ImmutableLeaderboard> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));

                Assert.That(gameSessionBL.Get(leaderBoardEntries[0].GameSessionId).UserId, Is.EqualTo(user1.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[1].GameSessionId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[2].GameSessionId).UserId, Is.EqualTo(user3.Id));

                Assert.That(leaderBoardEntries[0].Rank, Is.EqualTo(1));
                Assert.That(leaderBoardEntries[1].Rank, Is.EqualTo(2));
                Assert.That(leaderBoardEntries[2].Rank, Is.EqualTo(3));
            });
        }

        [Test]
        public void RankingTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user1 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession1 = TestHelper.AddGameSession(applicationContext, user1);
            ImmutableLeaderboard leaderboard1 = TestHelper.AddLeaderboard(applicationContext, gameSession1);

            ImmutableUser user2 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:101");
            ImmutableGameSession gameSession2 = TestHelper.AddGameSession(applicationContext, user2);
            ImmutableLeaderboard leaderboard2 = TestHelper.AddLeaderboard(applicationContext, gameSession2);

            ImmutableUser user3 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:102");
            ImmutableGameSession gameSession3 = TestHelper.AddGameSession(applicationContext, user3);
            ImmutableLeaderboard leaderboard3 = TestHelper.AddLeaderboard(applicationContext, gameSession3);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);

            leaderboadBL.AddScore(user1.Id, 10);
            leaderboadBL.AddScore(user2.Id, 25);
            leaderboadBL.AddScore(user3.Id, 15);

            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);

            IReadOnlyList<ImmutableLeaderboard> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[0].GameSessionId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[1].GameSessionId).UserId, Is.EqualTo(user3.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[2].GameSessionId).UserId, Is.EqualTo(user1.Id));

                Assert.That(leaderBoardEntries[0].Rank, Is.EqualTo(1));
                Assert.That(leaderBoardEntries[1].Rank, Is.EqualTo(2));
                Assert.That(leaderBoardEntries[2].Rank, Is.EqualTo(3));
            });
        }

        [Test]
        public void RankingWithMultipleScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user1 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSession gameSession1 = TestHelper.AddGameSession(applicationContext, user1);
            ImmutableLeaderboard leaderboard1 = TestHelper.AddLeaderboard(applicationContext, gameSession1);

            ImmutableUser user2 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:101");
            ImmutableGameSession gameSession2 = TestHelper.AddGameSession(applicationContext, user2);
            ImmutableLeaderboard leaderboard2 = TestHelper.AddLeaderboard(applicationContext, gameSession2);

            ImmutableUser user3 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:102");
            ImmutableGameSession gameSession3 = TestHelper.AddGameSession(applicationContext, user3);
            ImmutableLeaderboard leaderboard3 = TestHelper.AddLeaderboard(applicationContext, gameSession3);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);

            leaderboadBL.AddScore(user1.Id, 10);
            leaderboadBL.AddScore(user2.Id, 25);
            leaderboadBL.AddScore(user3.Id, 15);

            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);

            IReadOnlyList<ImmutableLeaderboard> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[0].GameSessionId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[1].GameSessionId).UserId, Is.EqualTo(user3.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[2].GameSessionId).UserId, Is.EqualTo(user1.Id));
            });

            leaderboadBL.AddScore(user1.Id, -5); // 5
            leaderboadBL.AddScore(user2.Id, 10); //35
            leaderboadBL.AddScore(user3.Id, 10); //25

            leaderBoardEntries = leaderboadBL.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[0].GameSessionId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[1].GameSessionId).UserId, Is.EqualTo(user3.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[2].GameSessionId).UserId, Is.EqualTo(user1.Id));
            });

            leaderboadBL.AddScore(user1.Id, 25); //30
            leaderboadBL.AddScore(user2.Id, -5); //30
            leaderboadBL.AddScore(user3.Id, -5); //20

            leaderBoardEntries = leaderboadBL.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[0].GameSessionId).UserId, Is.EqualTo(user1.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[1].GameSessionId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSessionBL.Get(leaderBoardEntries[2].GameSessionId).UserId, Is.EqualTo(user3.Id));

                Assert.That(leaderBoardEntries[0].Rank, Is.EqualTo(1));
                Assert.That(leaderBoardEntries[1].Rank, Is.EqualTo(2));
                Assert.That(leaderBoardEntries[2].Rank, Is.EqualTo(3));
            });
        }
    }
}