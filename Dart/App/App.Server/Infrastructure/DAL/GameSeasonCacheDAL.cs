using Core.Domain.Core;
using Core.Domain.Model;

namespace App.Server.Infrastructure.DAL
{
    public class GameSeasonCacheDAL
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
    }
}
