using Server.Domain;
using Server.Infrastructure.DAL;

namespace Server.Infrastructure.BL
{
    public class UserBL
    {
        private static readonly UserDALProxy userDAL = new UserDALProxy();

        public Guid Add(string endPoint)
        {
            User user = new User()
            {
                EndPoint = endPoint
            };

            userDAL.Add(user);

            return user.Id;
        }
    }
}
