using Core.Domain.Model;

namespace Core.Domain.Core
{
    public class ApplicationCache
    {
        public List<User> User { get; } = new List<User>(100);

        public List<GameSession> GameSession { get; } = new List<GameSession>(100);

        public List<Leaderboard> Leaderboard { get; } = new List<Leaderboard>(100);

        public Dictionary<Guid, List<Score>> Score { get; } = new Dictionary<Guid, List<Score>>(100);
    }
}
