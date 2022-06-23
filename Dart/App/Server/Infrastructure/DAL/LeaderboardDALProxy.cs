using Server.Domain;

namespace Server.Infrastructure.DAL
{
    public class LeaderboardDALProxy
    {
        private static readonly List<LeaderBoardEntry> _leaderboard = new List<LeaderBoardEntry>();

        public void Add(LeaderBoardEntry entry)
        {
            /*
             * TODO:
             * Do binary search instead of linear!
             */

            entry.Id = Guid.NewGuid();

            int indexToAdd = 0;
            for (int i = 0; i < _leaderboard.Count; ++i)
            {
                if (_leaderboard[i].Score < entry.Score)
                {
                    indexToAdd = i;
                }
            }

            entry.Rank = indexToAdd + 1;

            _leaderboard.Insert(indexToAdd, entry);
        }

        internal IReadOnlyList<LeaderBoardEntry> GetAll()
        {
            return _leaderboard;
        }
    }
}
