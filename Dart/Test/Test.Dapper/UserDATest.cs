using Core.Dapper;
using NUnit.Framework;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.Dapper
{
    public class UserDATest
    {
        #region Fields
        private readonly IApplicationContext _applicationContext = new ApplicationContext();
        #endregion

        #region Public Methods
        [Test]
        public void AddUserTest()
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

            User retrivedUser = userDA.Get(user.Id);

            Assert.That(user.Id, Is.EqualTo(retrivedUser.Id));
            Assert.That(user.Username, Is.EqualTo(retrivedUser.Username));
            Assert.That(user.EndPoint, Is.EqualTo(retrivedUser.EndPoint));
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

            User user2 = new User
            {
                Id = Guid.NewGuid(),
                Username = Guid.NewGuid().ToString(),
                EndPoint = "1.1.1.1:100"
            };

            User user3 = new User
            {
                Id = Guid.NewGuid(),
                Username = Guid.NewGuid().ToString(),
                EndPoint = "1.1.1.1:100"
            };

            UserDA userDA = new UserDA(_applicationContext);
            userDA.Add(user1);
            userDA.Add(user2);
            userDA.Add(user3);

            Assert.That(userDA.GetAll().Count(), Is.GreaterThan(3));
        }
        #endregion
    }
}