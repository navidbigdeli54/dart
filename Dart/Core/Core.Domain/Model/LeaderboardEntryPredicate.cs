namespace Core.Domain.Model
{
    public class LeaderboardEntryPredicate
    {
        public static Predicate<Leaderboard> FindUpperRank(Leaderboard entry)
        {
            return x => entry.Score <= x.Score;
        }
    }
}
