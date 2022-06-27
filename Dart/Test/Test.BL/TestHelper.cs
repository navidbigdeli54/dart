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

        public static ImmutableGameSession AddGameSession(IApplicationContext applicationContext, ImmutableUser user)
        {
            GameSessionBL gameSessionBL = new GameSessionBL(applicationContext);
            IResult<Guid> result = gameSessionBL.Add(user.Id);
            Assert.That(result.IsSuccessful, Is.True);

            return gameSessionBL.Get(result.Message);
        }

        public static ImmutableLeaderboard AddLeaderboard(IApplicationContext applicationContext, ImmutableGameSession gameSession)
        {
            LeaderboadBL leaderboardBL = new LeaderboadBL(applicationContext);
            IResult<Guid> result = leaderboardBL.Add(gameSession.Id);
            Assert.That(result.IsSuccessful, Is.True);

            return leaderboardBL.Get(result.Message);
        }

        public static ImmutableScore AddScore(IApplicationContext applicationContext, ImmutableGameSession gameSession, int point)
        {
            ScoreBL scoreBL = new ScoreBL(applicationContext);
            int randomPoint = Random.Shared.Next(0, int.MaxValue);
            IResult<Guid> result = scoreBL.Add(gameSession.Id, randomPoint);
            Assert.That(result.IsSuccessful, Is.True);

            return scoreBL.Get(result.Message);
        }
    }
}