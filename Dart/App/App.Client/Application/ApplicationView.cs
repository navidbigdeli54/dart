using Core.Domain.Model;

namespace App.Client.Application
{
    public class ApplicationView
    {
        #region Public Methods
        public void DrawHeader()
        {
            Console.Clear();
            Console.WriteLine($"# \t Name \tScore");
        }

        public void DisplayLeaderboard(IReadOnlyList<ImmutableUserLeaderboard> leaderboard)
        {
            DrawHeader();

            DrawLeaderboardList(leaderboard);
        }
        #endregion

        #region Private Methods
        private static void DrawLeaderboardList(IReadOnlyList<ImmutableUserLeaderboard> leaderboard)
        {
            for (int i = 0; i < leaderboard.Count; ++i)
            {
                ImmutableUserLeaderboard entry = leaderboard[i];
                Console.WriteLine($"{entry.LeaderboardEntry.Rank}\t {entry.User.Username}\t {entry.LeaderboardEntry.Score}");
            }
        }
        #endregion
    }
}
