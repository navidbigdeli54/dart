using Core.Dapper;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class UserCacheDAL : IDbSynchronizable
    {
        #region Fields
        private IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public UserCacheDAL(IApplicationContext applicationContext)
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

        #region IDbSynchronizable Implementation
        void IDbSynchronizable.Load()
        {
            UserDA userDA = new UserDA(_applicationContext);
            IReadOnlyList<User> users = userDA.GetAll();
            for (int i = 0; i < users.Count; ++i)
            {
                User user = users[i];
                _applicationContext.ApplicationCache.User.Add(user);
                user.IsDirty = false;
            }
        }

        void IDbSynchronizable.Save()
        {
            UserDA userDA = new UserDA(_applicationContext);
            for (int i = 0; i < _applicationContext.ApplicationCache.User.Count; ++i)
            {
                User user = _applicationContext.ApplicationCache.User[i];
                if (user.IsDirty)
                {
                    IResult result = userDA.Add(user);
                    if (result.IsSuccessful)
                    {
                        user.IsDirty = false;
                    }
                }
            }
        }
        #endregion
    }
}
