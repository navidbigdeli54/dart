using Server.Domain.Model;

namespace Server.Application
{
    public class ApplicationCache
    {
        public List<User> User { get; } = new List<User>(100);

        public List<GameSeason> GameSeason { get; } = new List<GameSeason>(100);

        public List<LeaderBoardEntry> Leaderboard { get; } = new List<LeaderBoardEntry>(100);
    }
}
