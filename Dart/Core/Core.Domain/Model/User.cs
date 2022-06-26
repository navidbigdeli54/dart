namespace Core.Domain.Model
{
    public class User
    {
        #region Fields
        private Guid _id;

        private string _username;

        private string _endPoint;
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

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                IsDirty = true;
            }
        }

        public string EndPoint
        {
            get => _endPoint; 
            set
            {
                _endPoint = value;
                IsDirty = true;
            }
        }

        public bool IsDirty { get; set; } = true;
        #endregion
    }
}
