using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class GameSessionCache
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public GameSessionCache(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public GameSession? Get(Guid gameSessionId)
        {
            return _applicationContext.ApplicationCache.GameSession.Where(x => x.Id == gameSessionId).SingleOrDefault();
        }

        public GameSession? GetByUserId(Guid userId)
        {
            return _applicationContext.ApplicationCache.GameSession.Where(x => x.UserId == userId).SingleOrDefault();
        }

        public IResult<Guid> Add(GameSession gameSession)
        {
            try
            {
                gameSession.Id = Guid.NewGuid();
                _applicationContext.ApplicationCache.GameSession.Add(gameSession);
                return new Result<Guid>(gameSession.Id);
            }
            catch (Exception exception)
            {
                return new ErrorResult<Guid>(new List<string> { "Can't add game season!", exception.Message });
            }
        }
        #endregion
    }
}
