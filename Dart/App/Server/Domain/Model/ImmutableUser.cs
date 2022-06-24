namespace Server.Domain.Model
{
    public struct ImmutableUser
    {
        #region Properties
        public Guid Id { get; }

        public string Username { get; }

        public string EndPoint { get; }

        public bool IsValid => Id != Guid.Empty;
        #endregion

        #region Constructors
        public ImmutableUser(User user)
        {
            Id = user.Id;
            Username = user.Username;
            EndPoint = user.EndPoint;
        }
        #endregion
    }
}
