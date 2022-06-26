using Core.Dapper;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class GameSeasonDbSynchronizer : IDbSynchronizer
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public GameSeasonDbSynchronizer(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region IDbSynchronizable Implementation
        void IDbSynchronizer.Load()
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

        void IDbSynchronizer.Save()
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
