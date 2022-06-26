namespace Core.Domain.Model
{
    public class GameSeason
    {
        #region Fields
        private Guid _id;

        private DateTime _creationDate;

        private Guid _userId;
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

        public Guid UserId
        {
            get=> _userId;
            set
            {
                _userId = value;
                IsDirty = true;
            }
        }

        public bool IsDirty { get; set; } = true;
        #endregion
    }
}
