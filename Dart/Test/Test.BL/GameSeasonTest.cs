using Core.BL;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.BL
{
    public class GameSeasonTest
    {
        [Test]
        public void AddGameSeasonTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, string.Empty);

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            IResult<Guid> result = gameSeasonBL.Add(user.Id);
            Assert.That(result.IsSuccessful, Is.True);

            ImmutableGameSeason gameSeason = gameSeasonBL.Get(result.Message);
            Assert.Multiple(() =>
            {
                Assert.That(gameSeason.Id, Is.EqualTo(result.Message));
                Assert.That(gameSeason.UserId, Is.EqualTo(user.Id));
                Assert.That(gameSeason.Scores.Select(x => x.Point).Sum(), Is.EqualTo(0));
            });
        }

        [Test]
        public void AddGameSeasonWithoutUserTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            IResult<Guid> result = gameSeasonBL.Add(Guid.Empty);
            Assert.That(result.IsSuccessful, Is.False);
        }

        [Test]
        public void AddGameSeasonForAUserTwiceTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, string.Empty);
            ImmutableGameSeason gameSeason = TestHelper.AddGameSeason(applicationContext, user);

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            IResult<Guid> result = gameSeasonBL.Add(user.Id);
            Assert.That(result.IsSuccessful, Is.False);
        }

        [Test]
        public void GetByUserIdTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            ImmutableUser user = TestHelper.AddUser(applicationContext, string.Empty, string.Empty);
            ImmutableGameSeason gameSeason = TestHelper.AddGameSeason(applicationContext, user);

            GameSeasonBL gameSeasonBL = new GameSeasonBL(applicationContext);
            ImmutableGameSeason retrivedUGameSeason = gameSeasonBL.GetByUserId(user.Id);
            Assert.Multiple(() =>
            {
                Assert.That(retrivedUGameSeason.Id, Is.EqualTo(gameSeason.Id));
                Assert.That(retrivedUGameSeason.UserId, Is.EqualTo(gameSeason.UserId));
                Assert.That(retrivedUGameSeason.Scores.Select(x => x.Point).Sum(), Is.EqualTo(gameSeason.Scores.Select(x => x.Point).Sum()));
            });
        }
    }
}