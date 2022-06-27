using Core.Cache;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.BL
{
    public class ScoreBL
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;

        private readonly ScoreCache _scoreCache;
        #endregion

        #region Constructors
        public ScoreBL(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _scoreCache = new ScoreCache(applicationContext);
        }
        #endregion

        #region Public Methods
        public ImmutableScore Get(Guid id)
        {
            Score? score = _scoreCache.Get(id);
            if (score != null)
            {
                return new ImmutableScore(score);
            }

            return default;
        }

        public IReadOnlyList<ImmutableScore> GetByGameSeasonId(Guid gameSeasonId)
        {
            return _scoreCache.GetByGameSeasonId(gameSeasonId).Select(x => new ImmutableScore(x)).ToList();
        }

        public IResult<Guid> Add(Guid gameSeasonId, int point)
        {
            GameSeasonBL gameSeasonBL = new GameSeasonBL(_applicationContext);
            ImmutableGameSeason gameSeason = gameSeasonBL.Get(gameSeasonId);
            if (gameSeason.IsValid)
            {
                Score score = new Score
                {
                    CreationDate = DateTime.UtcNow,
                    GameSeasonId = gameSeasonId,
                    Point = point
                };

                return _scoreCache.Add(score);
            }
            else
            {
                return new ErrorResult<Guid>($"Can't find {gameSeasonId} game season.");
            }
        }
        #endregion
    }
}
