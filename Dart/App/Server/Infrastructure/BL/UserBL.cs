using Server.Application;
using Server.Domain.Model;
using Server.Domain.Core;
using Server.Infrastructure.DAL;

namespace Server.Infrastructure.BL
{
    public class UserBL
    {
        #region Fields
        private readonly UserCacheDAL _userDAL;
        #endregion

        #region Constructors
        public UserBL(ApplicationContext applicationContext)
        {
            _userDAL = new UserCacheDAL(applicationContext);
        }
        #endregion

        #region Public Methods
        public ImmutableUser Get(Guid userId)
        {
            User? user = _userDAL.Get(userId);
            if (user != null)
            {
                return new ImmutableUser(user);
            }

            return default;
        }

        public IResult<Guid> Add(string username, string endPoint)
        {
            User user = new User()
            {
                Username = username,
                EndPoint = endPoint
            };

            return _userDAL.Add(user);
        }
        #endregion
    }
}

