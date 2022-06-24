using Server.Application;
using Server.Domain.Core;
using Server.Domain.Model;

namespace Server.Infrastructure.DAL
{
    public class GameSeasonCacheDAL
    {
        #region Fields
        private readonly ApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public GameSeasonCacheDAL(ApplicationContext applicationContext)
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

        public IResult AddScore(Guid gameSeasonId, int score)
        {
            GameSeason? gameSeason = Get(gameSeasonId);
            if (gameSeason != null)
            {
                gameSeason.Scores.Add(score);

                return new Result<object>();
            }
            else
            {
                return new ErrorResult<object>($"Can't find GameSeason by `{gameSeasonId}` id!");
            }
        }
        #endregion
    }
}
