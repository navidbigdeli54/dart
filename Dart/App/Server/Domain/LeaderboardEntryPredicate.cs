namespace Server.Domain
{
    public class LeaderboardEntryPredicate
    {
        public static Predicate<LeaderBoardEntry> FindUpperRank(LeaderBoardEntry entry)
        {
            return x => entry.Score < x.Score
                || (entry.Score == x.Score && entry.GameSeason?.User.Username.CompareTo(x.GameSeason?.User.Username) > 0);
        }
    }
}
