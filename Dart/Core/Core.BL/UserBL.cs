using Core.Cache;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.BL
{
    public class UserBL
    {
        #region Fields
        private readonly UserCache _userCache;
        #endregion

        #region Constructors
        public UserBL(IApplicationContext applicationContext)
        {
            _userCache = new UserCache(applicationContext);
        }
        #endregion

        #region Public Methods
        public ImmutableUser Get(Guid userId)
        {
            User? user = _userCache.Get(userId);
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

            return _userCache.Add(user);
        }
        #endregion
    }
}

