using Server.Domain;

namespace Server.Infrastructure.DAL
{
    public class LeaderboardDALProxy
    {
        #region Fields
        private readonly ApplicationContext _aplicationContext;
        #endregion

        #region Constructors
        public LeaderboardDALProxy(ApplicationContext applicationContext)
        {
            _aplicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public IReadOnlyList<LeaderBoardEntry> GetAll()
        {
            return _aplicationContext.ApplicationCache.Leaderboard;
        }

        public LeaderBoardEntry? GetByGameSeasonId(Guid gameSeasonId)
        {
            return _aplicationContext.ApplicationCache.Leaderboard.Where(x => x.GameSeason?.Id == gameSeasonId).SingleOrDefault();
        }

        public void Add(LeaderBoardEntry entry)
        {
            entry.Id = Guid.NewGuid();

            int indexToAdd = _aplicationContext.ApplicationCache.Leaderboard.FindIndex(LeaderboardEntryPredicate.FindUpperRank(entry)) + 1;

            _aplicationContext.ApplicationCache.Leaderboard.Insert(indexToAdd, entry);

            for (int i = indexToAdd; i < _aplicationContext.ApplicationCache.Leaderboard.Count; ++i)
            {
                _aplicationContext.ApplicationCache.Leaderboard[i].Rank = i + 1;
            }
        }

        public void UpdateScore(Guid gameSeasonId, int score)
        {
            LeaderBoardEntry? entry = GetByGameSeasonId(gameSeasonId);
            if (entry != null)
            {
                entry.Score = score;

                int previousIndex = entry.Rank - 1;

                _aplicationContext.ApplicationCache.Leaderboard.Remove(entry);

                int newIndex = _aplicationContext.ApplicationCache.Leaderboard.FindIndex(x => x.Score < entry.Score);
                if (newIndex < 0)
                {
                    newIndex = 0;
                }

                _aplicationContext.ApplicationCache.Leaderboard.Insert(newIndex, entry);

                int indexToUpdateFrom = previousIndex > newIndex ? previousIndex : newIndex;

                for (int i = indexToUpdateFrom; i < _aplicationContext.ApplicationCache.Leaderboard.Count; ++i)
                {
                    _aplicationContext.ApplicationCache.Leaderboard[i].Rank = i + 1;
                }
            }
        }
        #endregion
    }
}
