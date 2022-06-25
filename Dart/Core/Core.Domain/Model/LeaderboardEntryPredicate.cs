namespace Core.Domain.Model
{
    public class LeaderboardEntryPredicate
    {
        public static Predicate<LeaderboardEntry> FindUpperRank(LeaderboardEntry entry)
        {
            return x => entry.Score <= x.Score;
        }
    }
}
