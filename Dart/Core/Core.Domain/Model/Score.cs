namespace Core.Domain.Model
{
    public class Score
    {
        #region Fields
        private Guid _id;

        private DateTime _creationDate;

        public Guid _gameSeasonId;

        private int _point;
        #endregion

        #region Properties
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                IsDirty = true;
            }
        }

        public DateTime CreationDate
        {
            get => _creationDate;
            set
            {
                _creationDate = value;
                IsDirty = true;
            }
        }

        public Guid GameSeasonId
        {
            get => _gameSeasonId;
            set
            {
                _gameSeasonId = value;
                IsDirty = true;
            }
        }

        public int Point
        {
            get => _point;
            set
            {
                _point = value;
                IsDirty = true;
            }
        }

        public bool IsDirty { get; set; } = true;
        #endregion
    }
}
