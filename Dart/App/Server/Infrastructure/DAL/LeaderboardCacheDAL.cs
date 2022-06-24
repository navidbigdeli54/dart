﻿using Server.Application;
using Server.Domain.Core;
using Server.Domain.Model;

namespace Server.Infrastructure.DAL
{
    public class LeaderboardCacheDAL
    {
        #region Fields
        private readonly ApplicationContext _aplicationContext;
        #endregion

        #region Constructors
        public LeaderboardCacheDAL(ApplicationContext applicationContext)
        {
            _aplicationContext = applicationContext;
        }
        #endregion

        #region Public Methods
        public IReadOnlyList<LeaderBoardEntry> Get(int count)
        {
            return _aplicationContext.ApplicationCache.Leaderboard.Take(count).ToList();
        }

        public IReadOnlyList<LeaderBoardEntry> GetAll()
        {
            return _aplicationContext.ApplicationCache.Leaderboard;
        }

        public LeaderBoardEntry? GetByGameSeasonId(Guid gameSeasonId)
        {
            return _aplicationContext.ApplicationCache.Leaderboard.Where(x => x.GameSeasonId == gameSeasonId).SingleOrDefault();
        }

        public IResult<Guid> Add(LeaderBoardEntry entry)
        {
            try
            {
                entry.Id = Guid.NewGuid();

                int indexToAdd = _aplicationContext.ApplicationCache.Leaderboard.FindLastIndex(LeaderboardEntryPredicate.FindUpperRank(entry)) + 1;

                _aplicationContext.ApplicationCache.Leaderboard.Insert(indexToAdd, entry);

                for (int i = indexToAdd; i < _aplicationContext.ApplicationCache.Leaderboard.Count; ++i)
                {
                    _aplicationContext.ApplicationCache.Leaderboard[i].Rank = i + 1;
                }

                return new Result<Guid>(entry.Id);
            }
            catch (Exception exception)
            {
                return new ErrorResult<Guid>(new List<string> { "Can't add leaderboard!", exception.Message });
            }
        }

        public IResult UpdateScore(Guid gameSeasonId, int score)
        {
            try
            {
                LeaderBoardEntry? entry = GetByGameSeasonId(gameSeasonId);

                if (entry == null) throw new Exception($"Can't find GameSeason by `{gameSeasonId}` id!");

                entry.Score = score;

                _aplicationContext.ApplicationCache.Leaderboard.Remove(entry);

                int indexToAdd = _aplicationContext.ApplicationCache.Leaderboard.FindLastIndex(LeaderboardEntryPredicate.FindUpperRank(entry)) + 1;

                _aplicationContext.ApplicationCache.Leaderboard.Insert(indexToAdd, entry);

                int previousIndex = entry.Rank - 1;

                int updateIndex = previousIndex > indexToAdd ? indexToAdd : previousIndex;

                for (int i = updateIndex; i < _aplicationContext.ApplicationCache.Leaderboard.Count; ++i)
                {
                    _aplicationContext.ApplicationCache.Leaderboard[i].Rank = i + 1;
                }

                return new Result<object>();
            }
            catch (Exception exception)
            {
                return new ErrorResult<object>(new List<string> { "Can't update score!", exception.Message });
            }
        }
        #endregion
    }
}
