using Core.Domain.Core;


namespace Test.Dapper
{
    public class ApplicationContext : IApplicationContext
    {
        #region IApplicationContext Implementation
        string IApplicationContext.DBConnectionString { get; } = "Host=localhost;Database=DartDBTest;Username=postgres;Password=abcd1234";

        ApplicationCache IApplicationContext.ApplicationCache { get; } = null;

        DatabaseSynchronizer IApplicationContext.DatabaseSynchronizer { get; } = null;
        #endregion
    }
}