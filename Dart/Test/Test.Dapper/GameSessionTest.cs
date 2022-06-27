using Core.Dapper;
using NUnit.Framework;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.Dapper
{
    public class GameSessionTest
    {
        #region Fields
        private readonly IApplicationContext _applicationContext = new ApplicationContext();
        #endregion

        #region Public Methods
        [Test]
        public void AddGameSessionTest()
        {
            GameSession gameSession = new GameSession
            {
                Id = Guid.NewGuid(),
                UserId = TestHelper.AddUser(_applicationContext).Id,
                CreationDate = DateTime.UtcNow
            };
            GameSessionDA gameSessionDA = new GameSessionDA(_applicationContext);
            IResult result = gameSessionDA.Add(gameSession);
            Assert.That(result.IsSuccessful, Is.True);

            GameSession retrivedGameSession = gameSessionDA.Get(gameSession.Id);
            Assert.That(gameSession.Id, Is.EqualTo(retrivedGameSession.Id));
            Assert.That(gameSession.UserId, Is.EqualTo(retrivedGameSession.UserId));
        }

        [Test]
        public void GetAllTest()
        {
            TestHelper.ClearTable(_applicationContext, nameof(GameSession));

            TestHelper.AddGameSession(_applicationContext, TestHelper.AddUser(_applicationContext));
            TestHelper.AddGameSession(_applicationContext, TestHelper.AddUser(_applicationContext));
            TestHelper.AddGameSession(_applicationContext, TestHelper.AddUser(_applicationContext));

            GameSessionDA gameSessionDA = new GameSessionDA(_applicationContext);
            Assert.That(3, Is.EqualTo(gameSessionDA.GetAll().Count()));
        }
        #endregion
    }
}