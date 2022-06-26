using Core.Cache;
using Core.Domain.Core;

namespace App.Server.Application
{
    public class ApplicationContext : IApplicationContext
    {
        #region Fields
        private readonly ApplicationCache _applicationCache;

        private readonly DatabaseSynchronizer _databaseSynchronizer;
        #endregion

        #region Constructors
        public ApplicationContext()
        {
            _applicationCache = new ApplicationCache();

            _databaseSynchronizer = new DatabaseSynchronizer(new List<IDbSynchronizer>
            {
                new UserDbSynchronizer(this),
                new GameSeasonDbSynchronizer(this),
                new LeaderboardDbSynchronizer(this),
                new ScoreDbSynchronizer(this),
            });
        }
        #endregion

        #region IApplicationContext Implementation
        /*
         * Surely we should not put connection string in the app!
         */
        string IApplicationContext.DBConnectionString { get; } = "Host=localhost;Database=DartDB;Username=postgres;Password=abcd1234;";

        ApplicationCache IApplicationContext.ApplicationCache { get; } = new ApplicationCache();

        DatabaseSynchronizer IApplicationContext.DatabaseSynchronizer { get; }
        #endregion
    }
}
