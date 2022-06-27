using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class UserCache
    {
        #region FieldsGameSeasonCacheDAL
        private IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public UserCache(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public User? Get(Guid id)
        {
            return _applicationContext.ApplicationCache.User.Where(x => x.Id == id).SingleOrDefault();
        }

        public IResult<Guid> Add(User user)
        {
            try
            {
                user.Id = Guid.NewGuid();
                _applicationContext.ApplicationCache.User.Add(user);

                return new Result<Guid>(user.Id);
            }
            catch (Exception exception)
            {
                return new ErrorResult<Guid>(new List<string> { "Can't add new user!", exception.Message });
            }
        }
        #endregion
    }
}
