using Core.Dapper;
using Core.Domain.Core;
using Core.Domain.Model;

namespace Core.Cache
{
    public class LeaderboardDbSynchronizer : IDbSynchronizer
    {
        #region Fields
        private readonly IApplicationContext _aplicationContext;
        #endregion

        #region Constructors
        public LeaderboardDbSynchronizer(IApplicationContext applicationContext)
        {
            _aplicationContext = applicationContext;
        }
        #endregion

        #region IDbSynchronizable Implementation
        void IDbSynchronizer.Load()
        {
            LeaderboardDA leaderboardDA = new LeaderboardDA(_aplicationContext);
            IReadOnlyList<Leaderboard> leaderboards = leaderboardDA.GetAll();
            for (int i = 0; i < leaderboards.Count; ++i)
            {
                Leaderboard leaderboard = leaderboards[i];
                _aplicationContext.ApplicationCache.Leaderboard.Add(leaderboard);
                leaderboard.IsDirty = false;
            }

            _aplicationContext.ApplicationCache.Leaderboard.Sort();
        }

        void IDbSynchronizer.Save()
        {
            LeaderboardDA leaderboardDA = new LeaderboardDA(_aplicationContext);
            for (int i = 0; i < _aplicationContext.ApplicationCache.Leaderboard.Count; ++i)
            {
                Leaderboard leaderboard = _aplicationContext.ApplicationCache.Leaderboard[i];
                if (leaderboard.IsDirty)
                {
                    IResult result = leaderboardDA.UpdateOrAdd(leaderboard);
                    if (result.IsSuccessful)
                    {
                        leaderboard.IsDirty = false;
                    }
                }
            }
        }
        #endregion
    }
}
