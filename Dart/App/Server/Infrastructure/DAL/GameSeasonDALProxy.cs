using Server.Domain;

namespace Server.Infrastructure.DAL
{
    public class GameSeasonDALProxy
    {
        #region Fields
        private readonly ApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public GameSeasonDALProxy(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public GameSeason? Get(Guid gameSeasonId)
        {
            return _applicationContext.ApplicationCache.GameSeason.Where(x => x.Id == gameSeasonId).SingleOrDefault();
        }

        public GameSeason? GetByUserId(Guid userId)
        {
            return _applicationContext.ApplicationCache.GameSeason.Where(x => x.User.Id == userId).SingleOrDefault();
        }

        public void Add(GameSeason gameSeason)
        {
            gameSeason.Id = Guid.NewGuid();

            _applicationContext.ApplicationCache.GameSeason.Add(gameSeason);
        }

        public void AddScore(Guid gameSeasonId, int score)
        {
            GameSeason? gameSeason = Get(gameSeasonId);
            if (gameSeason != null)
            {
                gameSeason.Scores.Add(score);
            }
        }
        #endregion
    }
}
