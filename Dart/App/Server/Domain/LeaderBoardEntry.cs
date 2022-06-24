namespace Server.Domain
{
    public class LeaderBoardEntry : IComparable<LeaderBoardEntry>
    {
        #region Properties
        public Guid Id { get; set; }

        public GameSeason? GameSeason { get; set; }

        public int Rank { get; set; }

        public int Score { get; set; }
        #endregion

        #region IComparer<LeaderBoardEntry> Implementation
        int IComparable<LeaderBoardEntry>.CompareTo(LeaderBoardEntry? other)
        {
            if (Score > other?.Score) return 1;
            else if (Score < other?.Score) return -1;
            else return GameSeason?.User.Username.CompareTo(other?.GameSeason?.User.Username) ?? 0;
        }
        #endregion
    }
}
