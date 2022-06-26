namespace Core.Domain.Model
{
    public class GameSeason
    {
        #region Properties
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid UserId { get; set; }

        public bool IsDirty { get; set; } = true;
        #endregion
    }
}
