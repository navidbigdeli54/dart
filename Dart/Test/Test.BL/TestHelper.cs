using Core.BL;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.BL
{
    public static class TestHelper
    {
        public static ImmutableUser AddUser(IApplicationContext applicationContext, string username, string endPoint)
        {
            UserBL userBL = new UserBL(applicationContext);
            IResult<Guid> result = userBL.Add(Guid.NewGuid().ToString(), "[::1]:100");
            Assert.That(result.IsSuccessful, Is.True);

            return userBL.Get(result.Message);
        }

        public static ImmutableGameSeason AddGameSeason(IApplicationContext applicationContext, ImmutableUser user)
        {
            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            IResult<Guid> result = gameSeasonBL.Add(user.Id);
            Assert.That(result.IsSuccessful, Is.True);

            return gameSeasonBL.Get(result.Message);
        }

        public static ImmutableLeaderboard AddLeaderboard(IApplicationContext applicationContext, ImmutableGameSeason gameSeason)
        {
            LeaderboadBL leaderboardBL = new LeaderboadBL(applicationContext);
            IResult<Guid> result = leaderboardBL.Add(gameSeason.Id);
            Assert.That(result.IsSuccessful, Is.True);

            return leaderboardBL.Get(result.Message);
        }

        public static ImmutableScore AddScore(IApplicationContext applicationContext, ImmutableGameSeason gameSeason, int point)
        {
            ScoreBL scoreBL = new ScoreBL(applicationContext);
            int randomPoint = Random.Shared.Next(0, int.MaxValue);
            IResult<Guid> result = scoreBL.Add(gameSeason.Id, randomPoint);
            Assert.That(result.IsSuccessful, Is.True);

            return scoreBL.Get(result.Message);
        }
    }
}