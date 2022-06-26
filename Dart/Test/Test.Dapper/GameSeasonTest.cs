using Core.Dapper;
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
            User user = new User
            {
                Id = Guid.NewGuid(),
                Username = Guid.NewGuid().ToString(),
                EndPoint = "1.1.1.1:100"
            };
            UserDA userDA = new UserDA(_applicationContext);
            IResult result = userDA.Add(user);
            Assert.That(result.IsSuccessful, Is.True);

            GameSeason gameSeason = new GameSeason
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                CreationDate = DateTime.UtcNow
            };
            GameSeasonDA gameSeasonDA = new GameSeasonDA(_applicationContext);
            result = gameSeasonDA.Add(gameSeason);
            Assert.That(result.IsSuccessful, Is.True);

            GameSeason retrivedGameSeason = gameSeasonDA.Get(gameSeason.Id);

            Assert.That(retrivedGameSeason.Id, Is.EqualTo(gameSeason.Id));
            Assert.That(retrivedGameSeason.UserId, Is.EqualTo(gameSeason.UserId));
            Assert.That(retrivedGameSeason.CreationDate, Is.EqualTo(gameSeason.CreationDate));
        }

        [Test]
        public void GetAllTest()
        {
            User user1 = new User
            {
                Id = Guid.NewGuid(),
                Username = Guid.NewGuid().ToString(),
                EndPoint = "1.1.1.1:100"
            };
            UserDA userDA = new UserDA(_applicationContext);
            IResult result = userDA.Add(user1);
            Assert.That(result.IsSuccessful, Is.True);

            GameSeason gameSeason1 = new GameSeason
            {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                CreationDate = DateTime.UtcNow
            };
            GameSeasonDA gameSeasonDA = new GameSeasonDA(_applicationContext);
            result = gameSeasonDA.Add(gameSeason1);
            Assert.That(result.IsSuccessful, Is.True);

            User user2 = new User
            {
                Id = Guid.NewGuid(),
                Username = Guid.NewGuid().ToString(),
                EndPoint = "1.1.1.1:100"
            };
            result = userDA.Add(user2);
            Assert.That(result.IsSuccessful, Is.True);

            GameSeason gameSeason2 = new GameSeason
            {
                Id = Guid.NewGuid(),
                UserId = user2.Id,
                CreationDate = DateTime.UtcNow
            };
            result = gameSeasonDA.Add(gameSeason2);
            Assert.That(result.IsSuccessful, Is.True);

            User user3 = new User
            {
                Id = Guid.NewGuid(),
                Username = Guid.NewGuid().ToString(),
                EndPoint = "1.1.1.1:100"
            };
            result = userDA.Add(user3);
            Assert.That(result.IsSuccessful, Is.True);

            GameSeason gameSeason3 = new GameSeason
            {
                Id = Guid.NewGuid(),
                UserId = user3.Id,
                CreationDate = DateTime.UtcNow
            };
            result = gameSeasonDA.Add(gameSeason3);
            Assert.That(result.IsSuccessful, Is.True);

            Assert.That(gameSeasonDA.GetAll().Count(), Is.GreaterThan(3));
        }
        #endregion
    }
}