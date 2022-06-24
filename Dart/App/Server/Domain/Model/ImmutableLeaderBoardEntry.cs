namespace Server.Domain.Model
{
    public struct ImmutableLeaderBoardEntry
    {
        #region Properties
        public Guid Id { get; }

        public Guid GameSeasonId { get; }

        public int Score { get; }

        public int Rank { get; }

        public bool IsValid => Id != Guid.Empty;
        #endregion

        #region Properties
        public ImmutableLeaderBoardEntry(LeaderBoardEntry entry)
        {
            Id = entry.Id;
            Score = entry.Score;
            Rank = entry.Rank;
            GameSeasonId = entry.GameSeasonId;
        }
        #endregion
    }
}
