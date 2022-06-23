using Server.Domain;

namespace Server.Infrastructure.DAL
{
    public class UserDALProxy
    {
        private static readonly List<User> _users = new List<User>();

        public void Add(User user)
        {
            user.Id = Guid.NewGuid();

            _users.Add(user);
        }

        public User? Get(Guid id)
        {
            return _users.Where(x => x.Id == id).SingleOrDefault();
        }
    }
}
