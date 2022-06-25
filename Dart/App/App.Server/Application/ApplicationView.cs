using Core.Domain.Model;

namespace App.Server.Application
{
    public class ApplicationView
    {
        #region Public Methods
        public void DrawHeader()
        {
            Console.Clear();
            Console.WriteLine($"# \t                 Id                  \tScore");
        }

        public void DisplayLeaderboard(IReadOnlyList<ImmutableLeaderboardEntry> leaderboard)
        {
            DrawHeader();

            DrawLeaderboardList(leaderboard);
        }
        #endregion

        #region Private Methods
        private static void DrawLeaderboardList(IReadOnlyList<ImmutableLeaderboardEntry> leaderboard)
        {
            for (int i = 0; i < leaderboard.Count; ++i)
            {
                var entry = leaderboard[i];
                Console.WriteLine($"{entry.Rank}\t {entry.Id} \t {entry.Score}");
            }
        }
        #endregion
    }
}
