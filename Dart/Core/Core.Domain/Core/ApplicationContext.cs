namespace Core.Domain.Core
{
    public class ApplicationContext : IApplicationContext
    {
        #region IApplicationContext Implementation
        string IApplicationContext.DBConnectionString { get; } = "Host=localhost;Database=DartDB;Username=postgres";

        ApplicationCache IApplicationContext.ApplicationCache { get; } = new ApplicationCache();
        #endregion
    }
}
