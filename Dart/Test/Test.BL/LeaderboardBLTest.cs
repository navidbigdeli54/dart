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
            ImmutableGameSeason gameSeason = TestHelper.AddGameSeason(applicationContext, user);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            IResult<Guid> result = leaderboadBL.Add(gameSeason.Id);
            Assert.That(result.IsSuccessful, Is.True);

            ImmutableLeaderboard leaderboard = leaderboadBL.Get(result.Message);
            Assert.Multiple(() =>
            {
                Assert.That(leaderboard.Id, Is.EqualTo(result.Message));
                Assert.That(leaderboard.GameSeasonId, Is.EqualTo(gameSeason.Id));
                Assert.That(leaderboard.Rank, Is.EqualTo(1));
                Assert.That(leaderboard.Score, Is.EqualTo(0));
            });
        }

        [Test]
        public void AddLeaderboardTwiceForAGameSeasonTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();
            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSeason gameSeason = TestHelper.AddGameSeason(applicationContext, user);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            IResult<Guid> result = leaderboadBL.Add(gameSeason.Id);
            Assert.That(result.IsSuccessful, Is.True);

            ImmutableLeaderboard leaderboard = leaderboadBL.Get(result.Message);
            Assert.Multiple(() =>
            {
                Assert.That(leaderboard.Id, Is.EqualTo(result.Message));
                Assert.That(leaderboard.GameSeasonId, Is.EqualTo(gameSeason.Id));
                Assert.That(leaderboard.Rank, Is.EqualTo(1));
                Assert.That(leaderboard.Score, Is.EqualTo(0));
            });

            result = leaderboadBL.Add(gameSeason.Id);
            Assert.That(result.IsSuccessful, Is.False);
        }

        [Test]
        public void AddLeaderboardWithoutGameSeasonTest()
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
            ImmutableGameSeason gameSeason1 = TestHelper.AddGameSeason(applicationContext, user1);
            ImmutableLeaderboard leaderboard1 = TestHelper.AddLeaderboard(applicationContext, gameSeason1);

            ImmutableUser user2 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:101");
            ImmutableGameSeason gameSeason2 = TestHelper.AddGameSeason(applicationContext, user2);
            ImmutableLeaderboard leaderboard2 = TestHelper.AddLeaderboard(applicationContext, gameSeason2);

            ImmutableUser user3 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:102");
            ImmutableGameSeason gameSeason3 = TestHelper.AddGameSeason(applicationContext, user3);
            ImmutableLeaderboard leaderboard3 = TestHelper.AddLeaderboard(applicationContext, gameSeason3);

            ImmutableUser user4 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:102");
            ImmutableGameSeason gameSeason4 = TestHelper.AddGameSeason(applicationContext, user4);
            ImmutableLeaderboard leaderboard4 = TestHelper.AddLeaderboard(applicationContext, gameSeason4);


            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            IReadOnlyList<ImmutableLeaderboard> leaderBoardEntries = leaderboadBL.Get(3);
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));

                Assert.That(gameSeasonBL.Get(leaderBoardEntries[0].GameSeasonId).UserId, Is.EqualTo(user1.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[1].GameSeasonId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[2].GameSeasonId).UserId, Is.EqualTo(user3.Id));

                Assert.That(leaderBoardEntries[0].Rank, Is.EqualTo(1));
                Assert.That(leaderBoardEntries[1].Rank, Is.EqualTo(2));
                Assert.That(leaderBoardEntries[2].Rank, Is.EqualTo(3));
            });
        }

        [Test]
        public void AddEntriesThatHasSameScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user1 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSeason gameSeason1 = TestHelper.AddGameSeason(applicationContext, user1);
            ImmutableLeaderboard leaderboard1 = TestHelper.AddLeaderboard(applicationContext, gameSeason1);

            ImmutableUser user2 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:101");
            ImmutableGameSeason gameSeason2 = TestHelper.AddGameSeason(applicationContext, user2);
            ImmutableLeaderboard leaderboard2 = TestHelper.AddLeaderboard(applicationContext, gameSeason2);

            ImmutableUser user3 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:102");
            ImmutableGameSeason gameSeason3 = TestHelper.AddGameSeason(applicationContext, user3);
            ImmutableLeaderboard leaderboard3 = TestHelper.AddLeaderboard(applicationContext, gameSeason3);


            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            IReadOnlyList<ImmutableLeaderboard> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));

                Assert.That(gameSeasonBL.Get(leaderBoardEntries[0].GameSeasonId).UserId, Is.EqualTo(user1.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[1].GameSeasonId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[2].GameSeasonId).UserId, Is.EqualTo(user3.Id));

                Assert.That(leaderBoardEntries[0].Rank, Is.EqualTo(1));
                Assert.That(leaderBoardEntries[1].Rank, Is.EqualTo(2));
                Assert.That(leaderBoardEntries[2].Rank, Is.EqualTo(3));
            });
        }

        [Test]
        public void AddScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user1 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSeason gameSeason1 = TestHelper.AddGameSeason(applicationContext, user1);
            ImmutableLeaderboard leaderboard1 = TestHelper.AddLeaderboard(applicationContext, gameSeason1);

            ImmutableUser user2 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:101");
            ImmutableGameSeason gameSeason2 = TestHelper.AddGameSeason(applicationContext, user2);
            ImmutableLeaderboard leaderboard2 = TestHelper.AddLeaderboard(applicationContext, gameSeason2);

            ImmutableUser user3 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:102");
            ImmutableGameSeason gameSeason3 = TestHelper.AddGameSeason(applicationContext, user3);
            ImmutableLeaderboard leaderboard3 = TestHelper.AddLeaderboard(applicationContext, gameSeason3);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);

            leaderboadBL.AddScore(user1.Id, 10);
            leaderboadBL.AddScore(user2.Id, 25);
            leaderboadBL.AddScore(user3.Id, 15);

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);

            IReadOnlyList<ImmutableLeaderboard> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[0].GameSeasonId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[1].GameSeasonId).UserId, Is.EqualTo(user3.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[2].GameSeasonId).UserId, Is.EqualTo(user1.Id));

                Assert.That(leaderBoardEntries[0].Rank, Is.EqualTo(1));
                Assert.That(leaderBoardEntries[1].Rank, Is.EqualTo(2));
                Assert.That(leaderBoardEntries[2].Rank, Is.EqualTo(3));
            });
        }

        [Test]
        public void AddScoreMultipleTimeTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user1 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:100");
            ImmutableGameSeason gameSeason1 = TestHelper.AddGameSeason(applicationContext, user1);
            ImmutableLeaderboard leaderboard1 = TestHelper.AddLeaderboard(applicationContext, gameSeason1);

            ImmutableUser user2 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:101");
            ImmutableGameSeason gameSeason2 = TestHelper.AddGameSeason(applicationContext, user2);
            ImmutableLeaderboard leaderboard2 = TestHelper.AddLeaderboard(applicationContext, gameSeason2);

            ImmutableUser user3 = TestHelper.AddUser(applicationContext, string.Empty, "[::1]:102");
            ImmutableGameSeason gameSeason3 = TestHelper.AddGameSeason(applicationContext, user3);
            ImmutableLeaderboard leaderboard3 = TestHelper.AddLeaderboard(applicationContext, gameSeason3);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);

            leaderboadBL.AddScore(user1.Id, 10);
            leaderboadBL.AddScore(user2.Id, 25);
            leaderboadBL.AddScore(user3.Id, 15);

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);

            IReadOnlyList<ImmutableLeaderboard> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[0].GameSeasonId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[1].GameSeasonId).UserId, Is.EqualTo(user3.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[2].GameSeasonId).UserId, Is.EqualTo(user1.Id));
            });

            leaderboadBL.AddScore(user1.Id, -5); // 5
            leaderboadBL.AddScore(user2.Id, 10); //35
            leaderboadBL.AddScore(user3.Id, 10); //25

            leaderBoardEntries = leaderboadBL.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[0].GameSeasonId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[1].GameSeasonId).UserId, Is.EqualTo(user3.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[2].GameSeasonId).UserId, Is.EqualTo(user1.Id));
            });

            leaderboadBL.AddScore(user1.Id, 25); //30
            leaderboadBL.AddScore(user2.Id, -5); //30
            leaderboadBL.AddScore(user3.Id, -5); //20

            leaderBoardEntries = leaderboadBL.GetAll();
            Assert.Multiple(() =>
            {
                Assert.That(leaderBoardEntries, Has.Count.EqualTo(3));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[0].GameSeasonId).UserId, Is.EqualTo(user1.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[1].GameSeasonId).UserId, Is.EqualTo(user2.Id));
                Assert.That(gameSeasonBL.Get(leaderBoardEntries[2].GameSeasonId).UserId, Is.EqualTo(user3.Id));

                Assert.That(leaderBoardEntries[0].Rank, Is.EqualTo(1));
                Assert.That(leaderBoardEntries[1].Rank, Is.EqualTo(2));
                Assert.That(leaderBoardEntries[2].Rank, Is.EqualTo(3));
            });
        }
    }
}