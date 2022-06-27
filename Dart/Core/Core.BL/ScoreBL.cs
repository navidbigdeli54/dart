using Core.Cache;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.BL
{
    public class ScoreBL
    {
        #region Fields
        private readonly ScoreCache _scoreCache;
        #endregion

        #region Constructors
        public ScoreBL(IApplicationContext applicationContext)
        {
            _scoreCache = new ScoreCache(applicationContext);
        }
        #endregion

        #region Public Methods
        public IReadOnlyList<ImmutableScore> GetByGameSeasonId(Guid gameSeasonId)
        {
            return _scoreCache.GetByGameSeasonId(gameSeasonId);
        }

        public IResult<Guid> Add(Guid gameSeasonId, int point)
        {
            Score score = new Score
            {
                CreationDate = DateTime.UtcNow,
                GameSeasonId = gameSeasonId,
                Point = point
            };

            return _scoreCache.Add(score);
        }
        #endregion
    }
}
