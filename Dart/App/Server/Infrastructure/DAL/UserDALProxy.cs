using Server.Domain;

namespace Server.Infrastructure.DAL
{
    public class UserDALProxy
    {
        #region Fields
        private ApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public UserDALProxy(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public User? Get(Guid id)
        {
            return _applicationContext.ApplicationCache.User.Where(x => x.Id == id).SingleOrDefault();
        }

        public void Add(User user)
        {
            user.Id = Guid.NewGuid();

            _applicationContext.ApplicationCache.User.Add(user);
        } 
        #endregion
    }
}
