namespace Server.Domain.Model
{
    public class GameSeason
    {
        #region Properties
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid UserId { get; set; }

        public List<int> Scores { get; set; } = new List<int>();
        #endregion
    }
}
