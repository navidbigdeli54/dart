namespace Core.Domain.Model
{
    public class Leaderboard
    {
        #region Properties
        public Guid Id { get; set; }

        public Guid GameSeasonId { get; set; }

        public int Rank { get; set; }

        public int Score { get; set; }
        #endregion
    }
}
