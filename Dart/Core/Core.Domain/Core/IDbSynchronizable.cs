namespace Core.Domain.Core
{
    public interface IDbSynchronizer
    {
        void Load();

        void Save();
    }
}
