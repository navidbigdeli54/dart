namespace Core.Domain.Core
{
    public class DatabaseSynchronizer
    {
        #region Fields
        private IReadOnlyList<IDbSynchronizable> _dbSynchronizables;
        #endregion

        #region Constructors
        public DatabaseSynchronizer(IReadOnlyList<IDbSynchronizable> dbSynchronizables)
        {
            _dbSynchronizables = dbSynchronizables;
        }
        #endregion

        #region Public Methods
        public void Load()
        {
            for (int i = 0; i < _dbSynchronizables.Count; ++i)
            {
                try
                {
                    _dbSynchronizables[i].Load();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }

        public void Save()
        {
            for (int i = 0; i < _dbSynchronizables.Count; ++i)
            {
                try
                {
                    _dbSynchronizables[i].Save();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
        #endregion
    }
}
