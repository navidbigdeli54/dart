namespace Core.Domain.Core
{
    public class DatabaseSynchronizer
    {
        #region Fields
        private IReadOnlyList<IDbSynchronizer> _dbSynchronizables;

        private readonly Timer _timer;
        #endregion

        #region Constructors
        public DatabaseSynchronizer(IReadOnlyList<IDbSynchronizer> dbSynchronizables)
        {
            _dbSynchronizables = dbSynchronizables;

            Load();

            _timer = new Timer(Save, null, 0, 10000);
        }
        #endregion

        #region Private Methods
        private void Load()
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

        private void Save(object _)
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
