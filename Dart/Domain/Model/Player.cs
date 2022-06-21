namespace Domain.Model
{
    public class Player
    {
        #region Properties
        public Guid Id { get; set; }

        public int Room { get; set; }

        public int Score { get; set; }
        #endregion
    }
}
