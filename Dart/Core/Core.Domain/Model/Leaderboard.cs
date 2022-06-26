namespace Core.Domain.Model
{
    public class Leaderboard
    {
        #region Fields
        private Guid _id;

        private Guid _gameSeasonId;

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

        public Guid GameSeasonId
        {
            get => _gameSeasonId;
            set
            {
                _gameSeasonId = value;
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
    }
}
