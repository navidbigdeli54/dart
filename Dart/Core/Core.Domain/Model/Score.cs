namespace Core.Domain.Model
{
    public class Score
    {
        #region Properties
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid GameSeasonId { get; set; }

        public int Point { get; set; }
        #endregion
    }
}
