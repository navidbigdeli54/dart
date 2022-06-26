namespace Core.Domain.Model
{
    public class User
    {
        #region Properties
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string EndPoint { get; set; }

        public bool IsDirty { get; set; } = true;
        #endregion
    }
}
