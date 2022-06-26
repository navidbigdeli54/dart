using Core.Domain.Core;


namespace Test.BL
{
    public class ApplicationContext : IApplicationContext
    {
        #region IApplicationContext Implementation
        string IApplicationContext.DBConnectionString { get; } = "Host=localhost;Database=DartDBTest;Username=postgres;Password=abcd1234";

        ApplicationCache IApplicationContext.ApplicationCache { get; } = new ApplicationCache();

        DatabaseSynchronizer IApplicationContext.DatabaseSynchronizer { get; } = null;
        #endregion
    }
}