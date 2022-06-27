using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class ScoreCacheDAL
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public ScoreCacheDAL(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public IReadOnlyList<ImmutableScore> GetByGameSeasonId(Guid gameSeasonId)
        {
            if (_applicationContext.ApplicationCache.Score.TryGetValue(gameSeasonId, out List<Score> scores))
            {
                return scores.Select(x => new ImmutableScore(x)).ToList();
            }

            return new List<ImmutableScore>();
        }

        public IResult<Guid> Add(Score score)
        {
            try
            {
                score.Id = Guid.NewGuid();

                if (_applicationContext.ApplicationCache.Score.TryGetValue(score.GameSeasonId, out List<Score> scores))
                {
                    scores.Add(score);
                }
                else
                {
                    scores = new List<Score> { score };
                    _applicationContext.ApplicationCache.Score.Add(score.GameSeasonId, scores);
                }

                return new Result<Guid>(score.Id);
            }
            catch (Exception exception)
            {
                return new ErrorResult<Guid>(new List<string> { "Can't add score", exception.Message });
            }
        }
        #endregion
    }
}
