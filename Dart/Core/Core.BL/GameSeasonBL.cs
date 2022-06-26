using Core.Domain.Core;
using Core.Domain.Model;
using Core.Cache;

namespace Core.BL
{
    public class GameSeasonBL
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;

        private readonly GameSeasonCacheDAL _gameSeasonDAL;
        #endregion

        #region Constructors
        public GameSeasonBL(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _gameSeasonDAL = new GameSeasonCacheDAL(applicationContext);
        }
        #endregion

        #region Public Methods
        public ImmutableGameSeason Get(Guid gameSeasonId)
        {
            GameSeason? gameSeason = _gameSeasonDAL.Get(gameSeasonId);
            if (gameSeason != null)
            {
                ScoreBL scoreBL = new ScoreBL(_applicationContext);
                IReadOnlyList<ImmutableScore> scores = scoreBL.GetByGameSeasonId(gameSeasonId);

                return new ImmutableGameSeason(gameSeason, scores);
            }

            return default;
        }

        public ImmutableGameSeason GetByUserId(Guid userId)
        {
            GameSeason? gameSeason = _gameSeasonDAL.GetByUserId(userId);
            if (gameSeason != null)
            {
                ScoreBL scoreBL = new ScoreBL(_applicationContext);
                IReadOnlyList<ImmutableScore> scores = scoreBL.GetByGameSeasonId(gameSeason.Id);

                return new ImmutableGameSeason(gameSeason, scores);
            }

            return default;
        }

        public IResult<Guid> Add(Guid userId)
        {
            UserBL userBL = new UserBL(_applicationContext);
            ImmutableUser user = userBL.Get(userId);

            if (user.IsValid)
            {
                GameSeason gameSeason = new GameSeason
                {
                    CreationDate = DateTime.UtcNow,
                    UserId = user.Id
                };

                return _gameSeasonDAL.Add(gameSeason);
            }

            return new ErrorResult<Guid>($"Can't find User by `{userId}` id!");
        }

        public IResult AddNewScore(Guid userId, int score)
        {
            ImmutableGameSeason gameSeason = GetByUserId(userId);
            if (gameSeason.IsValid && gameSeason.Scores.Count < 10 && gameSeason.CreationDate - DateTime.UtcNow < TimeSpan.FromMinutes(2))
            {
                ScoreBL scoreBL = new ScoreBL(_applicationContext);
                return scoreBL.Add(gameSeason.Id, score);
            }
            else
            {
                return new ErrorResult<object>("Can't add new score!");
            }
        }
        #endregion
    }
}
