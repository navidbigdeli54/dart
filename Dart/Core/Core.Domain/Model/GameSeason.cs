namespace Domain.Model
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

    public class Score
    {
        #region Properties
        public int Id { get; set; }

        public Guid GameSeasonId { get; set; }

        public int Point { get; set; }
        #endregion
    }
}
