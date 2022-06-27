using Core.Dapper;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class GameSessionDbSynchronizer : IDbSynchronizer
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public GameSessionDbSynchronizer(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region IDbSynchronizable Implementation
        void IDbSynchronizer.Load()
        {
            GameSessionDA gameSessionDA = new GameSessionDA(_applicationContext);
            IReadOnlyList<GameSession> gameSessions = gameSessionDA.GetAll();
            for (int i = 0; i < gameSessions.Count; ++i)
            {
                GameSession gameSession = gameSessions[i];
                _applicationContext.ApplicationCache.GameSession.Add(gameSession);
                gameSession.IsDirty = false;
            }
        }

        void IDbSynchronizer.Save()
        {
            GameSessionDA gameSessionDA = new GameSessionDA(_applicationContext);
            for (int i = 0; i < _applicationContext.ApplicationCache.GameSession.Count; ++i)
            {
                GameSession gameSession = _applicationContext.ApplicationCache.GameSession[i];
                if (gameSession.IsDirty)
                {
                    gameSessionDA.Add(gameSession);
                    gameSession.IsDirty = false;
                }
            }
        }
        #endregion
    }
}
