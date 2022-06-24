namespace Server.Domain.Model
{
    public struct ImmutableGameSeason
    {
        #region Public Methods
        public Guid Id { get; }

        public DateTime CreationDate { get; }

        public Guid UserId { get; }

        public IReadOnlyList<int> Scores { get; }

        public bool IsValid => Id != Guid.Empty;
        #endregion

        #region Constructors
        public ImmutableGameSeason(GameSeason gameSeason)
        {
            Id = gameSeason.Id;
            CreationDate = gameSeason.CreationDate;
            UserId = gameSeason.UserId;
            Scores = new List<int>(gameSeason.Scores);
        }
        #endregion
    }
}
