namespace Core.Domain.Core
{
    public interface IDbSynchronizable
    {
        void Load();

        void Save();
    }
}
