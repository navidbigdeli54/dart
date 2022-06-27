﻿using Core.Domain.Core;
using Core.Domain.Model;
using Core.Cache;

namespace Core.BL
{
    public class LeaderboadBL
    {
        #region Fields
        private readonly IApplicationContext _applicationContext;

        private readonly LeaderboardCacheDAL _leaderboardCacheDAL;
        #endregion

        #region Constructors
        public LeaderboadBL(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
            _leaderboardCacheDAL = new LeaderboardCacheDAL(applicationContext);
        }
        #endregion

        #region Public Methods
        public IReadOnlyList<ImmutableLeaderboard> Get(int count)
        {
            return _leaderboardCacheDAL.Get(count).Select(x => new ImmutableLeaderboard(x)).ToList();
        }

        public IReadOnlyList<ImmutableLeaderboard> GetAll()
        {
            return _leaderboardCacheDAL.GetAll().Select(x => new ImmutableLeaderboard(x)).ToList();
        }

        public IResult<Guid> Add(Guid gameSeasonId)
        {
            GameSeasonBL gameSeasonBL = new GameSeasonBL(_applicationContext);
            ImmutableGameSeason gameSeason = gameSeasonBL.Get(gameSeasonId);
            if (gameSeason.IsValid)
            {
                Leaderboard leaderBoardEntry = new Leaderboard
                {
                    GameSeasonId = gameSeason.Id
                };

                return _leaderboardCacheDAL.Add(leaderBoardEntry);
            }
            else
            {
                return new ErrorResult<Guid>($"Can't find a GameSeason by `{gameSeasonId}` id.");
            }
        }

        public IResult AddScore(Guid userId, int score)
        {
            GameSeasonBL gameSeasonBL = new GameSeasonBL(_applicationContext);
            IResult result = gameSeasonBL.AddNewScore(userId, score);

            if (result.IsSuccessful)
            {
                ImmutableGameSeason gameSeason = gameSeasonBL.GetByUserId(userId);

                ScoreBL scoreBL = new ScoreBL(_applicationContext);

                _leaderboardCacheDAL.UpdateScore(gameSeason.Id, scoreBL.GetByGameSeasonId(gameSeason.Id).Select(x => x.Point).Sum());

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