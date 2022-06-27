using System.Reflection;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class ScoreCache
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;
        #endregion

        #region Constructors
        public ScoreCache(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public Score? Get(Guid id)
        {
            foreach (KeyValuePair<Guid, List<Score>> pair in _applicationContext.ApplicationCache.Score)
            {
                List<Score> scores = pair.Value;
                Score? score = scores.SingleOrDefault(x => x.Id == id);
                if (score != null)
                {
                    return score;
                }
            }

            return null;
        }

        public IReadOnlyList<Score> GetByGameSeasonId(Guid gameSeasonId)
        {
            if (_applicationContext.ApplicationCache.Score.TryGetValue(gameSeasonId, out List<Score>? scores))
            {
                return scores;
            }

            return new List<Score>();
        }

        public IResult<Guid> Add(Score score)
        {
            try
            {
                score.Id = Guid.NewGuid();

                if (_applicationContext.ApplicationCache.Score.TryGetValue(score.GameSeasonId, out List<Score>? scores))
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
