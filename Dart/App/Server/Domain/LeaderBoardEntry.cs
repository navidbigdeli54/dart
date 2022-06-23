namespace Server.Domain
{
    public class LeaderBoardEntry
    {
        #region Properties
        public Guid Id { get; set; }

        public GameSeason GameSeason { get; set; }

        public int Rank { get; set; }

        public int Score { get; set; }
        #endregion
    }
}
