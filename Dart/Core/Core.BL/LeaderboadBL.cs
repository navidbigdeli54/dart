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

        public IResult<Guid> Add(Guid gameSeasonId)
        {
            GameSeasonBL gameSeasonBL = new GameSeasonBL(_applicationContext);
            ImmutableGameSeason gameSeason = gameSeasonBL.Get(gameSeasonId);
            if (gameSeason.IsValid)
            {
                Leaderboard? existedLeaderbaord = _leaderboardCache.GetByGameSeasonId(gameSeasonId);
                if (existedLeaderbaord == null)
                {
                    Leaderboard leaderBoardEntry = new Leaderboard
                    {
                        GameSeasonId = gameSeason.Id
                    };

                    return _leaderboardCache.Add(leaderBoardEntry);
                }

                return new ErrorResult<Guid>($"A leaderboard for the `{gameSeasonId}` game season is already exist!");
            }
            else
            {
                return new ErrorResult<Guid>($"Can't find a GameSeason by `{gameSeasonId}` id.");
            }
        }

        public IResult AddScore(Guid userId, int point)
        {
            GameSeasonBL gameSeasonBL = new GameSeasonBL(_applicationContext);
            IResult result = gameSeasonBL.AddNewScore(userId, point);
            if (result.IsSuccessful)
            {
                ImmutableGameSeason gameSeason = gameSeasonBL.GetByUserId(userId);

                ScoreBL scoreBL = new ScoreBL(_applicationContext);

                IReadOnlyList<ImmutableScore> scores = scoreBL.GetByGameSeasonId(gameSeason.Id);

                _leaderboardCache.UpdateScore(gameSeason.Id, scores.Select(x => x.Point).Sum());

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
