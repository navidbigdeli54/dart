using Server.Domain;
using Server.Infrastructure.DAL;

namespace Server.Infrastructure.BL
{
    public class UserBL
    {
        #region Fields
        private readonly UserDALProxy _userDAL;
        #endregion

        #region Constructors
        public UserBL(ApplicationContext applicationContext)
        {
            _userDAL = new UserDALProxy(applicationContext);
        }
        #endregion

        #region Public Methods
        public Guid Add(string username, string endPoint)
        {
            User user = new User()
            {
                Username = username,
                EndPoint = endPoint
            };

            _userDAL.Add(user);

            return user.Id;
        }
        #endregion
    }
}
