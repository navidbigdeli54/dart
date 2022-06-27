using Core.BL;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Test.BL
{
    public class UserBLTest
    {
        [Test]
        public void AddUserTest()
        {
            ApplicationContext applicationContext = new ApplicationContext();

            UserBL userBL = new UserBL(applicationContext);

            string username = Guid.NewGuid().ToString();
            string endPoint = "[::1]:100";

            IResult<Guid> result = userBL.Add(username, endPoint);
            Assert.That(result.IsSuccessful, Is.True);

            ImmutableUser retrivedUser = userBL.Get(result.Message);
            Assert.Multiple(() =>
            {
                Assert.That(retrivedUser.IsValid, Is.True);
                Assert.That(retrivedUser.Username, Is.EqualTo(username));
                Assert.That(retrivedUser.EndPoint, Is.EqualTo(endPoint));
            });
        }
    }
}