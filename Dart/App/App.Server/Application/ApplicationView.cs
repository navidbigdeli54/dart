using Core.BL;
using Core.Domain.Model;

namespace App.Server.Application
{
    public class ApplicationView
    {
        #region Public Methods
        public void DisplayLeaderboard()
        {
            DrawHeader();

            DrawLeaderboardList();
        }
        #endregion

        #region Private Methods
        private void DrawHeader()
        {
            Console.Clear();
            Console.WriteLine($"#\t Name\tScore");
        }

        private static void DrawLeaderboardList()
        {
            UserBL userBL = new UserBL(Program.ApplicationContext);
            LeaderboadBL leaderboadBL = new LeaderboadBL(Program.ApplicationContext);
            GameSeasonBL gameSeasonBL = new GameSeasonBL(Program.ApplicationContext);

            IReadOnlyList<ImmutableLeaderboard> allEntries = leaderboadBL.GetAll();
            for (int i = 0; i < allEntries.Count; i++)
            {
                ImmutableLeaderboard leaderboardEntry = allEntries[i];

                ImmutableGameSeason gameSeason = gameSeasonBL.Get(leaderboardEntry.GameSeasonId);

                ImmutableUser user = userBL.Get(gameSeason.UserId);

                Console.WriteLine($"{leaderboardEntry.Rank}\t {user.Username}\t {leaderboardEntry.Score}");
            }
        }
        #endregion
    }
}
