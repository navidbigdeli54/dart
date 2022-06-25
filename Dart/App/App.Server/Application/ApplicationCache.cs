using Domain.Model;

namespace Server.Application
{
    public class ApplicationCache
    {
        public List<User> User { get; } = new List<User>(100);

        public List<GameSeason> GameSeason { get; } = new List<GameSeason>(100);

        public List<LeaderboardEntry> Leaderboard { get; } = new List<LeaderboardEntry>(100);
    }
}
