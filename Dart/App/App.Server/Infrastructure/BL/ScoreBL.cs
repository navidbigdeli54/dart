using App.Server.Infrastructure.DAL;
using Core.Domain.Core;
using Core.Domain.Model;

namespace App.Server.Infrastructure.BL
{
    public class ScoreBL
    {
        #region Fields
        private readonly ScoreCacheDAL _scoreCacheDAL;
        #endregion

        #region Constructors
        public ScoreBL(IApplicationContext applicationContext)
        {
            _scoreCacheDAL = new ScoreCacheDAL(applicationContext);
        }
        #endregion

        #region Public Methods
        public IReadOnlyList<ImmutableScore> GetByGameSeasonId(Guid gameSeasonId)
        {
            return _scoreCacheDAL.GetByGameSeasonId(gameSeasonId);
        }

        public IResult<Guid> Add(Guid gameSeasonId, int point)
        {
            Score score = new Score
            {
                CreationDate = DateTime.UtcNow,
                GameSeasonId = gameSeasonId,
                Point = point
            };

            return _scoreCacheDAL.Add(score);
        }
        #endregion
    }
}
