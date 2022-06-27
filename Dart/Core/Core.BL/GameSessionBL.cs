using Core.Cache;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.BL
{
    public class GameSessionBL
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;

        private readonly GameSessionCache _gameSessionCache;
        #endregion

        #region Constructors
        public GameSessionBL(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _gameSessionCache = new GameSessionCache(applicationContext);
        }
        #endregion

        #region Public Methods
        public ImmutableGameSession Get(Guid gameSessionId)
        {
            GameSession? gameSession = _gameSessionCache.Get(gameSessionId);
            if (gameSession != null)
            {
                ScoreBL scoreBL = new ScoreBL(_applicationContext);
                IReadOnlyList<ImmutableScore> scores = scoreBL.GetByGameSessionId(gameSessionId);

                return new ImmutableGameSession(gameSession, scores);
            }

            return default;
        }

        public ImmutableGameSession GetByUserId(Guid userId)
        {
            GameSession? gameSession = _gameSessionCache.GetByUserId(userId);
            if (gameSession != null)
            {
                ScoreBL scoreBL = new ScoreBL(_applicationContext);
                IReadOnlyList<ImmutableScore> scores = scoreBL.GetByGameSessionId(gameSession.Id);

                return new ImmutableGameSession(gameSession, scores);
            }

            return default;
        }

        public IResult<Guid> Add(Guid userId)
        {
            UserBL userBL = new UserBL(_applicationContext);
            ImmutableUser user = userBL.Get(userId);

            if (user.IsValid)
            {
                ImmutableGameSession existedGameSession = GetByUserId(user.Id);
                if (existedGameSession.IsValid == false)
                {
                    GameSession gameSession = new GameSession
                    {
                        CreationDate = DateTime.UtcNow,
                        UserId = user.Id
                    };

                    return _gameSessionCache.Add(gameSession);
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
            ImmutableGameSession gameSession = GetByUserId(userId);
            if (gameSession.IsValid)
            {
                if (gameSession.Scores.Count < ImmutableGameSession.MAX_SCORE_NUMBER)
                {
                    TimeSpan difference = DateTime.UtcNow - gameSession.CreationDate;
                    if (difference <= ImmutableGameSession.MAX_PLAY_DURATION)
                    {
                        ScoreBL scoreBL = new ScoreBL(_applicationContext);
                        return scoreBL.Add(gameSession.Id, score);
                    }
                    else
                    {
                        return new ErrorResult<object>($"Can't play more than {ImmutableGameSession.MAX_PLAY_DURATION.TotalMinutes} minutes.");
                    }
                }
                else
                {
                    return new ErrorResult<object>($"Can't add more than {ImmutableGameSession.MAX_SCORE_NUMBER} scores.");
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
