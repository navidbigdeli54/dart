namespace Server.Domain.Model
{
    public class LeaderboardEntryPredicate
    {
        public static Predicate<LeaderBoardEntry> FindUpperRank(LeaderBoardEntry entry)
        {
            return x => entry.Score <= x.Score;
        }
    }
}
