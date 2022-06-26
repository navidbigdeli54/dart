using Dapper;
using Core.Dapper;
using System.Data;
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
            TestHelper.ClearTable(_applicationContext, nameof(User));

            TestHelper.AddUser(_applicationContext);
            TestHelper.AddUser(_applicationContext);
            TestHelper.AddUser(_applicationContext);

            UserDA userDA = new UserDA(_applicationContext);

            Assert.That(3, Is.EqualTo(userDA.GetAll().Count()));
        }
        #endregion
    }
}