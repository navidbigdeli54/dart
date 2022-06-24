using Server.Domain;
using Server.Infrastructure.DAL;

namespace Server.Infrastructure.BL
{
    public class GameSeasonBL
    {
        #region Fields
        private readonly ApplicationContext _applicationContext;

        private readonly GameSeasonDALProxy _gameSeasonDAL;
        #endregion

        #region Constructors
        public GameSeasonBL(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _gameSeasonDAL = new GameSeasonDALProxy(applicationContext);
        }
        #endregion

        #region Public Methods
        public GameSeason? Get(Guid gameSeasonId)
        {
            return _gameSeasonDAL.Get(gameSeasonId);
        }

        public GameSeason? GetByUserId(Guid userId)
        {
            return _gameSeasonDAL.GetByUserId(userId);
        }

        public Guid Add(Guid userGuid)
        {
            UserDALProxy userDALProxy = new UserDALProxy(_applicationContext);
            User? user = userDALProxy.Get(userGuid);

            if (user != null)
            {
                GameSeason gameSeason = new GameSeason
                {
                    CreationDate = DateTime.UtcNow,
                    User = user
                };

                _gameSeasonDAL.Add(gameSeason);

                return gameSeason.Id;
            }

            return Guid.Empty;
        }

        public GameSeason? AddScore(Guid userId, int score)
        {
            GameSeason? gameSeason = GetByUserId(userId);
            if (gameSeason != null && gameSeason.Scores.Count < 10 && gameSeason.CreationDate - DateTime.UtcNow < TimeSpan.FromMinutes(2))
            {
                _gameSeasonDAL.AddScore(gameSeason.Id, score);
            }

            return gameSeason;
        }
        #endregion
    }
}
