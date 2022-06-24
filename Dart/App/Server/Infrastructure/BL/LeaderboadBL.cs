using Server.Application;
using Server.Domain;
using Server.Infrastructure.DAL;

namespace Server.Infrastructure.BL
{
    public class LeaderboadBL
    {
        #region Fields
        private readonly ApplicationContext _applicationContext;

        private readonly LeaderboardCacheDAL _leaderboardDALProxy;
        #endregion

        #region Constructors
        public LeaderboadBL(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _leaderboardDALProxy = new LeaderboardCacheDAL(applicationContext);
        }
        #endregion

        #region Public Methods
        public IReadOnlyList<LeaderBoardEntry> GetAll()
        {
            return _leaderboardDALProxy.GetAll();
        }

        public Guid Add(Guid gameSeasonId)
        {
            GameSeasonBL gameSeasonBL = new GameSeasonBL(_applicationContext);
            GameSeason? gameSeason = gameSeasonBL.Get(gameSeasonId);
            if (gameSeason != null)
            {
                LeaderBoardEntry leaderBoardEntry = new LeaderBoardEntry
                {
                    GameSeason = gameSeason
                };

                _leaderboardDALProxy.Add(leaderBoardEntry);

                return leaderBoardEntry.Id;
            }

            return Guid.Empty;
        }

        public void AddScore(Guid userId, int score)
        {
            GameSeasonBL gameSeasonBL = new GameSeasonBL(_applicationContext);
            GameSeason? gameSeason = gameSeasonBL.AddScore(userId, score);
            if (gameSeason != null)
            {
                _leaderboardDALProxy.UpdateScore(gameSeason.Id, gameSeason.Scores.Sum());
            }
        }
        #endregion
    }
}
