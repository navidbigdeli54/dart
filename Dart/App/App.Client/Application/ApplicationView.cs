using Core.Domain.Model;

namespace App.Client.Application
{
    public class ApplicationView
    {
        #region Public Methods
        public void DrawHeader()
        {
            Console.Clear();
            Console.WriteLine($"# \t                 Id                  \tScore");
        }

        public void DisplayLeaderboard(IReadOnlyList<ImmutableUserLeaderboardEntry> leaderboard)
        {
            DrawHeader();

            DrawLeaderboardList(leaderboard);
        }
        #endregion

        #region Private Methods
        private static void DrawLeaderboardList(IReadOnlyList<ImmutableUserLeaderboardEntry> leaderboard)
        {
            for (int i = 0; i < leaderboard.Count; ++i)
            {
                var entry = leaderboard[i];
                Console.WriteLine($"{entry.LeaderboardEntry.Rank}\t {entry.LeaderboardEntry.Id} \t {entry.LeaderboardEntry.Score}");
            }
        }
        #endregion
    }
}
