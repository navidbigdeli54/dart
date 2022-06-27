namespace Core.Domain.Model
{
    public class Leaderboard : IComparable<Leaderboard>
    {
        #region Fields
        private Guid _id;

        private Guid _gameSessionId;

        private int _rank;

        private int _score;
        #endregion

        #region Properties
        public Guid Id
        {
            get => _id;
            set
            {
                _id = value;
                IsDirty = true;
            }
        }

        public Guid GameSessionId
        {
            get => _gameSessionId;
            set
            {
                _gameSessionId = value;
                IsDirty = true;
            }
        }

        public int Rank
        {
            get => _rank;
            set
            {
                _rank = value;
                IsDirty = true;
            }
        }

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                IsDirty = true;
            }
        }

        public bool IsDirty { get; set; } = true;
        #endregion

        #region IComparable<Leaderboard> Implementation
        int IComparable<Leaderboard>.CompareTo(Leaderboard? other)
        {
            if (Rank > other.Rank) return 1;
            else if (Rank < other.Rank) return -1;
            else return 0;
        }
        #endregion
    }
}
