using Server.Application;
using Server.Domain;
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
            Guid user1 = userBL.Add("A", "[::1]:100");
            Guid user2 = userBL.Add("Z", "[::1]:101");
            Guid user3 = userBL.Add("G", "[::1]:102");

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            Guid user1GameSeason = gameSeasonBL.Add(user1);
            Guid user2GameSeason = gameSeasonBL.Add(user2);
            Guid user3GameSeason = gameSeasonBL.Add(user3);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            Guid user1Leaderboard = leaderboadBL.Add(user1GameSeason);
            Guid user2Leaderboard = leaderboadBL.Add(user2GameSeason);
            Guid user3Leaderboard = leaderboadBL.Add(user3GameSeason);

            IReadOnlyList<LeaderBoardEntry> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.That(3, Is.EqualTo(leaderBoardEntries.Count));
            Assert.That(user1, Is.EqualTo(leaderBoardEntries[0].GameSeason?.User.Id));
            Assert.That(user3, Is.EqualTo(leaderBoardEntries[1].GameSeason?.User.Id));
            Assert.That(user2, Is.EqualTo(leaderBoardEntries[2].GameSeason?.User.Id));

            Assert.That(1, Is.EqualTo(leaderBoardEntries[0].Rank));
            Assert.That(2, Is.EqualTo(leaderBoardEntries[1].Rank));
            Assert.That(3, Is.EqualTo(leaderBoardEntries[2].Rank));
        }

        [Test]
        public void AddScoreTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            UserBL userBL = new UserBL(applicationContext);
            Guid user1 = userBL.Add("A", "[::1]:100");
            Guid user2 = userBL.Add("Z", "[::1]:101");
            Guid user3 = userBL.Add("G", "[::1]:102");

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            Guid user1GameSeason = gameSeasonBL.Add(user1);
            Guid user2GameSeason = gameSeasonBL.Add(user2);
            Guid user3GameSeason = gameSeasonBL.Add(user3);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            Guid user1Leaderboard = leaderboadBL.Add(user1GameSeason);
            Guid user2Leaderboard = leaderboadBL.Add(user2GameSeason);
            Guid user3Leaderboard = leaderboadBL.Add(user3GameSeason);

            leaderboadBL.AddScore(user1, 10);
            leaderboadBL.AddScore(user2, 25);
            leaderboadBL.AddScore(user3, 15);

            IReadOnlyList<LeaderBoardEntry> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.That(3, Is.EqualTo(leaderBoardEntries.Count));
            Assert.That(user2, Is.EqualTo(leaderBoardEntries[0].GameSeason?.User.Id));
            Assert.That(user3, Is.EqualTo(leaderBoardEntries[1].GameSeason?.User.Id));
            Assert.That(user1, Is.EqualTo(leaderBoardEntries[2].GameSeason?.User.Id));

            Assert.That(1, Is.EqualTo(leaderBoardEntries[0].Rank));
            Assert.That(2, Is.EqualTo(leaderBoardEntries[1].Rank));
            Assert.That(3, Is.EqualTo(leaderBoardEntries[2].Rank));
        }

        [Test]
        public void AddScoreMultipleTimeTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            UserBL userBL = new UserBL(applicationContext);
            Guid user1 = userBL.Add("A", "[::1]:100");
            Guid user2 = userBL.Add("Z", "[::1]:101");
            Guid user3 = userBL.Add("G", "[::1]:102");

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            Guid user1GameSeason = gameSeasonBL.Add(user1);
            Guid user2GameSeason = gameSeasonBL.Add(user2);
            Guid user3GameSeason = gameSeasonBL.Add(user3);

            LeaderboadBL leaderboadBL = new LeaderboadBL(applicationContext);
            Guid user1Leaderboard = leaderboadBL.Add(user1GameSeason);
            Guid user2Leaderboard = leaderboadBL.Add(user2GameSeason);
            Guid user3Leaderboard = leaderboadBL.Add(user3GameSeason);

            leaderboadBL.AddScore(user1, 10);
            leaderboadBL.AddScore(user2, 25);
            leaderboadBL.AddScore(user3, 15);

            IReadOnlyList<LeaderBoardEntry> leaderBoardEntries = leaderboadBL.GetAll();
            Assert.That(3, Is.EqualTo(leaderBoardEntries.Count));
            Assert.That(user2, Is.EqualTo(leaderBoardEntries[0].GameSeason?.User.Id));
            Assert.That(user3, Is.EqualTo(leaderBoardEntries[1].GameSeason?.User.Id));
            Assert.That(user1, Is.EqualTo(leaderBoardEntries[2].GameSeason?.User.Id));

            leaderboadBL.AddScore(user1, -5); // 5
            leaderboadBL.AddScore(user2, 10); //35
            leaderboadBL.AddScore(user3, 10); //25

            leaderBoardEntries = leaderboadBL.GetAll();
            Assert.That(3, Is.EqualTo(leaderBoardEntries.Count));
            Assert.That(user2, Is.EqualTo(leaderBoardEntries[0].GameSeason?.User.Id));
            Assert.That(user3, Is.EqualTo(leaderBoardEntries[1].GameSeason?.User.Id));
            Assert.That(user1, Is.EqualTo(leaderBoardEntries[2].GameSeason?.User.Id));

            leaderboadBL.AddScore(user1, 25); //30
            leaderboadBL.AddScore(user2, -5); //30
            leaderboadBL.AddScore(user3, -5); //20

            leaderBoardEntries = leaderboadBL.GetAll();
            Assert.That(3, Is.EqualTo(leaderBoardEntries.Count));
            Assert.That(user1, Is.EqualTo(leaderBoardEntries[0].GameSeason?.User.Id));
            Assert.That(user2, Is.EqualTo(leaderBoardEntries[1].GameSeason?.User.Id));
            Assert.That(user3, Is.EqualTo(leaderBoardEntries[2].GameSeason?.User.Id));

            Assert.That(1, Is.EqualTo(leaderBoardEntries[0].Rank));
            Assert.That(2, Is.EqualTo(leaderBoardEntries[1].Rank));
            Assert.That(3, Is.EqualTo(leaderBoardEntries[2].Rank));
        }
    }
}