using Core.Cache;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.BL
{
    public class LeaderboadBL
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;

        private readonly LeaderboardCache _leaderboardCache;
        #endregion

        #region Constructors
        public LeaderboadBL(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _leaderboardCache = new LeaderboardCache(applicationContext);
        }
        #endregion

        #region Public Methods
        public ImmutableLeaderboard Get(Guid id)
        {
            Leaderboard? leaderboard = _leaderboardCache.Get(id);
            if (leaderboard != null)
            {
                return new ImmutableLeaderboard(leaderboard);
            }

            return default;
        }

        public IReadOnlyList<ImmutableLeaderboard> Get(int count)
        {
            return _leaderboardCache.Get(count).Select(x => new ImmutableLeaderboard(x)).ToList();
        }

        public IReadOnlyList<ImmutableLeaderboard> GetAll()
        {
            return _leaderboardCache.GetAll().Select(x => new ImmutableLeaderboard(x)).ToList();
        }

        public IResult<Guid> Add(Guid gameSessionId)
        {
            GameSessionBL gameSessionBL = new GameSessionBL(_applicationContext);
            ImmutableGameSession gameSession = gameSessionBL.Get(gameSessionId);
            if (gameSession.IsValid)
            {
                Leaderboard? existedLeaderbaord = _leaderboardCache.GetByGameSessionId(gameSessionId);
                if (existedLeaderbaord == null)
                {
                    Leaderboard leaderBoardEntry = new Leaderboard
                    {
                        GameSessionId = gameSession.Id
                    };

                    return _leaderboardCache.Add(leaderBoardEntry);
                }

                return new ErrorResult<Guid>($"A leaderboard for the `{gameSessionId}` game season is already exist!");
            }
            else
            {
                return new ErrorResult<Guid>($"Can't find a GameSession by `{gameSessionId}` id.");
            }
        }

        public IResult AddScore(Guid userId, int point)
        {
            GameSessionBL gameSessionBL = new GameSessionBL(_applicationContext);
            IResult result = gameSessionBL.AddNewScore(userId, point);
            if (result.IsSuccessful)
            {
                ImmutableGameSession gameSession = gameSessionBL.GetByUserId(userId);

                ScoreBL scoreBL = new ScoreBL(_applicationContext);

                IReadOnlyList<ImmutableScore> scores = scoreBL.GetByGameSessionId(gameSession.Id);

                _leaderboardCache.UpdateScore(gameSession.Id, scores.Select(x => x.Point).Sum());

                return new Result<object>();
            }
            else
            {
                return result;
            }
        }
        #endregion
    }
}
