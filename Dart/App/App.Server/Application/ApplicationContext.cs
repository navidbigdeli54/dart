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

            _databaseSynchronizer = new DatabaseSynchronizer(new List<IDbSynchronizable>());
        }
        #endregion

        #region IApplicationContext Implementation
        /*
         * Surely we should not put connection string in the app!
         */
        string IApplicationContext.DBConnectionString { get; } = "Host=localhost;Database=DartDB;Username=postgres";

        ApplicationCache IApplicationContext.ApplicationCache { get; } = new ApplicationCache();

        DatabaseSynchronizer IApplicationContext.DatabaseSynchronizer { get; }
        #endregion
    }
}
