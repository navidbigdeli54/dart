namespace Server.Domain
{
    public class Player
    {
        #region Properties
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public int Room { get; set; }

        public List<int> TurnScores { get; set; } = new List<int>();

        public int TotalScore { get; set; }
        #endregion
    }
}
