using Core.Domain.Model;

namespace Core.Domain.Core
{
    public class ApplicationCache
    {
        public List<User> User { get; } = new List<User>(100);

        public List<GameSeason> GameSeason { get; } = new List<GameSeason>(100);

        public List<LeaderboardEntry> Leaderboard { get; } = new List<LeaderboardEntry>(100);

        public Dictionary<Guid, List<Score>> Score { get; } = new Dictionary<Guid, List<Score>>(100);
    }
}
