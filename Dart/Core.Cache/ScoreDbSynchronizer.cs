using Core.Dapper;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class ScoreDbSynchronizer : IDbSynchronizer
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;
        #endregion
        #region Constructors

        public ScoreDbSynchronizer(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        #endregion

        #region IDbSynchronizable Implementation
        void IDbSynchronizer.Load()
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

        void IDbSynchronizer.Save()
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
