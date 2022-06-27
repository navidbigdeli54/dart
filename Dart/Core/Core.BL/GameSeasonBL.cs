using Core.Cache;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.BL
{
    public class GameSeasonBL
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;

        private readonly GameSeasonCache _gameSeasonCache;
        #endregion

        #region Constructors
        public GameSeasonBL(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _gameSeasonCache = new GameSeasonCache(applicationContext);
        }
        #endregion

        #region Public Methods
        public ImmutableGameSeason Get(Guid gameSeasonId)
        {
            GameSeason? gameSeason = _gameSeasonCache.Get(gameSeasonId);
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
            GameSeason? gameSeason = _gameSeasonCache.GetByUserId(userId);
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
                ImmutableGameSeason existedGameSeason = GetByUserId(user.Id);
                if (existedGameSeason.IsValid == false)
                {
                    GameSeason gameSeason = new GameSeason
                    {
                        CreationDate = DateTime.UtcNow,
                        UserId = user.Id
                    };

                    return _gameSeasonCache.Add(gameSeason);
                }
                else
                {
                    return new ErrorResult<Guid>($"Can't add game season because {userId} already has one!");
                }
            }

            return new ErrorResult<Guid>($"Can't find User by `{userId}` id!");
        }

        public IResult AddNewScore(Guid userId, int score)
        {
            ImmutableGameSeason gameSeason = GetByUserId(userId);
            if (gameSeason.IsValid)
            {
                if (gameSeason.Scores.Count < ImmutableGameSeason.MAX_SCORE_NUMBER)
                {
                    if (gameSeason.CreationDate - DateTime.UtcNow <= ImmutableGameSeason.MAX_PLAY_DURATION)
                    {
                        ScoreBL scoreBL = new ScoreBL(_applicationContext);
                        return scoreBL.Add(gameSeason.Id, score);
                    }
                    else
                    {
                        return new ErrorResult<object>($"Can't play more than {ImmutableGameSeason.MAX_PLAY_DURATION.TotalMinutes} minutes.");
                    }
                }
                else
                {
                    return new ErrorResult<object>($"Can't add more than {ImmutableGameSeason.MAX_SCORE_NUMBER} scores.");
                }
            }
            else
            {
                return new ErrorResult<object>("Can't find game season!");
            }
        }
        #endregion
    }
}
