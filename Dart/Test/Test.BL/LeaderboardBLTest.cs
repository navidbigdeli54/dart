using Domain.Model;
using Server.Application;
using Server.Infrastructure.BL;

namespace Test.BL
{
    public class LeaderboardBLTest
    {
        [Test]
        public void AddEntriesThatHasSameScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            UserBL userBL = new UserBL(applicationContext);
            Guid user1 = userBL.Add(Guid.NewGuid().ToString(), "[::1]:100").Message;
            Guid user2 = userBL.Add(Guid.NewGuid().ToString(), "[::1]:101").Message;
            Guid user3 = userBL.Add(Guid.NewGuid().ToString(), "[::1]:102").Message;

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            Guid user1GameSeason = gameSeasonBL.Add(user1).Message;
            Guid user2GameSeason = gameSeasonBL.Add(user2).Message;
            Guid user3GameSeason = gameSeasonBL.Add(user3).Message;

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            Guid user1Leaderboard = leaderboadBL.Add(user1GameSeason).Message;
            Guid user2Leaderboard = leaderboadBL.Add(user2GameSeason).Message;
            Guid user3Leaderboard = leaderboadBL.Add(user3GameSeason).Message;

            IReadOnlyList<ImmutableLeaderboardEntry> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.That(3, Is.EqualTo(leaderBoardEntries.Count));
            Assert.That(user1, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[0].GameSeasonId).UserId));
            Assert.That(user2, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[1].GameSeasonId).UserId));
            Assert.That(user3, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[2].GameSeasonId).UserId));

            Assert.That(1, Is.EqualTo(leaderBoardEntries[0].Rank));
            Assert.That(2, Is.EqualTo(leaderBoardEntries[1].Rank));
            Assert.That(3, Is.EqualTo(leaderBoardEntries[2].Rank));
        }

        [Test]
        public void AddScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            UserBL userBL = new UserBL(applicationContext);
            Guid user1 = userBL.Add("A", "[::1]:100").Message;
            Guid user2 = userBL.Add("Z", "[::1]:101").Message;
            Guid user3 = userBL.Add("G", "[::1]:102").Message;

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            Guid user1GameSeason = gameSeasonBL.Add(user1).Message;
            Guid user2GameSeason = gameSeasonBL.Add(user2).Message;
            Guid user3GameSeason = gameSeasonBL.Add(user3).Message;

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            Guid user1Leaderboard = leaderboadBL.Add(user1GameSeason).Message;
            Guid user2Leaderboard = leaderboadBL.Add(user2GameSeason).Message;
            Guid user3Leaderboard = leaderboadBL.Add(user3GameSeason).Message;

            leaderboadBL.AddScore(user1, 10);
            leaderboadBL.AddScore(user2, 25);
            leaderboadBL.AddScore(user3, 15);

            IReadOnlyList<ImmutableLeaderboardEntry> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.That(3, Is.EqualTo(leaderBoardEntries.Count));
            Assert.That(user2, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[0].GameSeasonId).UserId));
            Assert.That(user3, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[1].GameSeasonId).UserId));
            Assert.That(user1, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[2].GameSeasonId).UserId));

            Assert.That(1, Is.EqualTo(leaderBoardEntries[0].Rank));
            Assert.That(2, Is.EqualTo(leaderBoardEntries[1].Rank));
            Assert.That(3, Is.EqualTo(leaderBoardEntries[2].Rank));
        }

        [Test]
        public void AddScoreMultipleTimeTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            UserBL userBL = new UserBL(applicationContext);
            Guid user1 = userBL.Add("A", "[::1]:100").Message;
            Guid user2 = userBL.Add("Z", "[::1]:101").Message;
            Guid user3 = userBL.Add("G", "[::1]:102").Message;

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            Guid user1GameSeason = gameSeasonBL.Add(user1).Message;
            Guid user2GameSeason = gameSeasonBL.Add(user2).Message;
            Guid user3GameSeason = gameSeasonBL.Add(user3).Message;

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            Guid user1Leaderboard = leaderboadBL.Add(user1GameSeason).Message;
            Guid user2Leaderboard = leaderboadBL.Add(user2GameSeason).Message;
            Guid user3Leaderboard = leaderboadBL.Add(user3GameSeason).Message;

            leaderboadBL.AddScore(user1, 10);
            leaderboadBL.AddScore(user2, 25);
            leaderboadBL.AddScore(user3, 15);

            IReadOnlyList<ImmutableLeaderboardEntry> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.That(3, Is.EqualTo(leaderBoardEntries.Count));
            Assert.That(user2, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[0].GameSeasonId).UserId));
            Assert.That(user3, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[1].GameSeasonId).UserId));
            Assert.That(user1, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[2].GameSeasonId).UserId));

            leaderboadBL.AddScore(user1, -5); // 5
            leaderboadBL.AddScore(user2, 10); //35
            leaderboadBL.AddScore(user3, 10); //25

            leaderBoardEntries = leaderboadBL.GetAll();
            Assert.That(3, Is.EqualTo(leaderBoardEntries.Count));
            Assert.That(user2, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[0].GameSeasonId).UserId));
            Assert.That(user3, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[1].GameSeasonId).UserId));
            Assert.That(user1, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[2].GameSeasonId).UserId));

            leaderboadBL.AddScore(user1, 25); //30
            leaderboadBL.AddScore(user2, -5); //30
            leaderboadBL.AddScore(user3, -5); //20

            leaderBoardEntries = leaderboadBL.GetAll();
            Assert.That(3, Is.EqualTo(leaderBoardEntries.Count));
            Assert.That(user1, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[0].GameSeasonId).UserId));
            Assert.That(user2, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[1].GameSeasonId).UserId));
            Assert.That(user3, Is.EqualTo(gameSeasonBL.Get(leaderBoardEntries[2].GameSeasonId).UserId));

            Assert.That(1, Is.EqualTo(leaderBoardEntries[0].Rank));
            Assert.That(2, Is.EqualTo(leaderBoardEntries[1].Rank));
            Assert.That(3, Is.EqualTo(leaderBoardEntries[2].Rank));
        }
    }
}