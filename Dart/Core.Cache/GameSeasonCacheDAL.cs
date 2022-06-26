using Core.Dapper;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class GameSeasonCacheDAL : IDbSynchronizable
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public GameSeasonCacheDAL(IApplicationContext applicationContext)
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
            return _applicationContext.ApplicationCache.GameSeason.Where(x => x.UserId == userId).SingleOrDefault();
        }

        public IResult<Guid> Add(GameSeason gameSeason)
        {
            try
            {
                gameSeason.Id = Guid.NewGuid();
                _applicationContext.ApplicationCache.GameSeason.Add(gameSeason);
                return new Result<Guid>(gameSeason.Id);
            }
            catch (Exception exception)
            {
                return new ErrorResult<Guid>(new List<string> { "Can't add game season!", exception.Message });
            }
        }
        #endregion

        #region IDbSynchronizable Implementation
        void IDbSynchronizable.Load()
        {
            GameSeasonDA gameSeasonDA = new GameSeasonDA(_applicationContext);
            IReadOnlyList<GameSeason> gameSeasons = gameSeasonDA.GetAll();
            for (int i = 0; i < gameSeasons.Count; ++i)
            {
                GameSeason gameSeason = gameSeasons[i];
                _applicationContext.ApplicationCache.GameSeason.Add(gameSeason);
                gameSeason.IsDirty = false;
            }
        }

        void IDbSynchronizable.Save()
        {
            GameSeasonDA gameSeasonDA = new GameSeasonDA(_applicationContext);
            for (int i = 0; i < _applicationContext.ApplicationCache.GameSeason.Count; ++i)
            {
                GameSeason gameSeason = _applicationContext.ApplicationCache.GameSeason[i];
                if (gameSeason.IsDirty)
                {
                    gameSeasonDA.Add(gameSeason);
                    gameSeason.IsDirty = false;
                }
            }
        }
        #endregion
    }
}
