using Server.Domain;

namespace Server.Infrastructure.DAL
{
    public class LeaderboardDALProxy
    {
        private static readonly List<LeaderBoardEntry> _leaderboard = new List<LeaderBoardEntry>();

        public LeaderBoardEntry? GetByGameSeasonId(Guid gameSeasonId)
        {
            return _leaderboard.Where(x => x.GameSeason.Id == gameSeasonId).SingleOrDefault();
        }

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

        public void UpdateScore(Guid gameSeasonId, int score)
        {
            LeaderBoardEntry? entry = GetByGameSeasonId(gameSeasonId);
            if (entry != null)
            {
                entry.Score = score;

                _leaderboard.Remove(entry);

                int indexToAdd = 0;
                for (int i = 0; i < _leaderboard.Count; ++i)
                {
                    if (_leaderboard[i].Score > entry.Score)
                    {
                        indexToAdd = i;
                    }
                }

                entry.Rank = indexToAdd + 1;

                _leaderboard.Insert(indexToAdd, entry);
            }
        }

        public IReadOnlyList<LeaderBoardEntry> GetAll()
        {
            return _leaderboard;
        }
    }
}
