using Core.Dapper;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class ScoreCacheDAL : IDbSynchronizable
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

        #region IDbSynchronizable Implementation
        void IDbSynchronizable.Load()
        {
            ScoreDA scoreDA = new ScoreDA(_applicationContext);
            IReadOnlyList<Score> scores = scoreDA.GetAll();
            for (int i = 0; i < scores.Count; ++i)
            {
                Score score = scores[i];
                if (_applicationContext.ApplicationCache.Score.TryGetValue(score.GameSeasonId, out List<Score> gameSeasonScores))
                {
                    gameSeasonScores.Add(score);
                }
                else
                {
                    gameSeasonScores = new List<Score> { score };
                    _applicationContext.ApplicationCache.Score.Add(score.GameSeasonId, gameSeasonScores);
                }

                score.IsDirty = false;
            }
        }

        void IDbSynchronizable.Save()
        {
            ScoreDA scoreDA = new ScoreDA(_applicationContext);
            foreach (KeyValuePair<Guid, List<Score>> pair in _applicationContext.ApplicationCache.Score)
            {
                List<Score> gameSeasonScores = pair.Value;
                for (int i = 0; i < gameSeasonScores.Count; ++i)
                {
                    Score score = gameSeasonScores[i];
                    if (score.IsDirty)
                    {
                        IResult result = scoreDA.Add(score);
                        if (result.IsSuccessful)
                        {
                            score.IsDirty = false;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
