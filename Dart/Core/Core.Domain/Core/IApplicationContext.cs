namespace Core.Domain.Core
{
    public interface IApplicationContext
    {
        public string DBConnectionString { get; }

        public ApplicationCache ApplicationCache { get; }

        public DatabaseSynchronizer DatabaseSynchronizer { get; }
    }
}
