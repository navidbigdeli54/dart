using Dapper;
using Core.Dapper;
using System.Data;
using NUnit.Framework;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.Dapper
{
    public class GameSeasonTest
    {
        #region Fields
        private readonly IApplicationContext _applicationContext = new ApplicationContext();
        #endregion

        #region Public Methods
        [Test]
        public void AddGameSeasonTest()
        {
            GameSeason gameSeason = new GameSeason
            {
                Id = Guid.NewGuid(),
                UserId = TestHelper.AddUser(_applicationContext).Id,
                CreationDate = DateTime.UtcNow
            };
            GameSeasonDA gameSeasonDA = new GameSeasonDA(_applicationContext);
            IResult result = gameSeasonDA.Add(gameSeason);
            Assert.That(result.IsSuccessful, Is.True);

            GameSeason retrivedGameSeason = gameSeasonDA.Get(gameSeason.Id);
            Assert.That(gameSeason.Id, Is.EqualTo(retrivedGameSeason.Id));
            Assert.That(gameSeason.UserId, Is.EqualTo(retrivedGameSeason.UserId));
        }

        [Test]
        public void GetAllTest()
        {
            TestHelper.ClearTable(_applicationContext, nameof(GameSeason));

            TestHelper.AddGameSeason(_applicationContext, TestHelper.AddUser(_applicationContext));
            TestHelper.AddGameSeason(_applicationContext, TestHelper.AddUser(_applicationContext));
            TestHelper.AddGameSeason(_applicationContext, TestHelper.AddUser(_applicationContext));

            GameSeasonDA gameSeasonDA = new GameSeasonDA(_applicationContext);
            Assert.That(3, Is.EqualTo(gameSeasonDA.GetAll().Count()));
        }
        #endregion
    }
}