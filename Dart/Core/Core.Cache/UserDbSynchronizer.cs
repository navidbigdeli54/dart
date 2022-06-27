using Core.Dapper;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class UserDbSynchronizer : IDbSynchronizer
    {
        #region Fields
        private IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public UserDbSynchronizer(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region IDbSynchronizable Implementation
        void IDbSynchronizer.Load()
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

        void IDbSynchronizer.Save()
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
