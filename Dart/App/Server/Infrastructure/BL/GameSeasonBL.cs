using Domain.Core;
using Domain.Model;
using Server.Application;
using Server.Infrastructure.DAL;

namespace Server.Infrastructure.BL
{
    public class GameSeasonBL
    {
        #region Fields
        private readonly ApplicationContext _applicationContext;

        private readonly GameSeasonCacheDAL _gameSeasonDAL;
        #endregion

        #region Constructors
        public GameSeasonBL(ApplicationContext applicationContext)
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
                return new ImmutableGameSeason(gameSeason);
            }

            return default;
        }

        public ImmutableGameSeason GetByUserId(Guid userId)
        {
            GameSeason? gameSeason = _gameSeasonDAL.GetByUserId(userId);
            if (gameSeason != null)
            {
                return new ImmutableGameSeason(gameSeason);
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
                _gameSeasonDAL.AddScore(gameSeason.Id, score);

                return new Result<object>();
            }
            else
            {
                return new ErrorResult<object>("Can't add new score!");
            }
        }
        #endregion
    }
}
