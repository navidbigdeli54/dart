namespace Server.Domain
{
    public class GameSeason
    {
        #region Properties
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public User User { get; set; }

        public List<int> Scores { get; set; }

        public int TotalScore => Scores.Sum();
        #endregion
    }
}
