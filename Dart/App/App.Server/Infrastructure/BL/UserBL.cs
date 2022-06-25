using App.Server.Infrastructure.DAL;
using Core.Domain.Core;
using Core.Domain.Model;

namespace App.Server.Infrastructure.BL
{
    public class UserBL
    {
        #region Fields
        private readonly UserCacheDAL _userDAL;
        #endregion

        #region Constructors
        public UserBL(IApplicationContext applicationContext)
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

