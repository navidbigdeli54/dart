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
            Console.WriteLine($"# \t Name \tScore");
        }

        private static void DrawLeaderboardList()
        {
            UserBL userBL = new UserBL(Program.ApplicationContext);
            LeaderboadBL leaderboadBL = new LeaderboadBL(Program.ApplicationContext);
            GameSeasonBL gameSeasonBL = new GameSeasonBL(Program.ApplicationContext);

            List<ImmutableUserLeaderboard> entriesToShow = new List<ImmutableUserLeaderboard>();
            IReadOnlyList<ImmutableLeaderboard> allEntries = leaderboadBL.GetAll();
            for (int i = 0; i < allEntries.Count; i++)
            {
                ImmutableLeaderboard leaderBoardEntry = allEntries[i];

                ImmutableGameSeason gameSeason = gameSeasonBL.Get(leaderBoardEntry.GameSeasonId);

                ImmutableUser user = userBL.Get(gameSeason.UserId);

                entriesToShow.Add(new ImmutableUserLeaderboard(user, leaderBoardEntry));
            }

            for (int i = 0; i < entriesToShow.Count; ++i)
            {
                ImmutableUserLeaderboard entry = entriesToShow[i];
                Console.WriteLine($"{entry.LeaderboardEntry.Rank}\t {entry.User.Username} \t {entry.LeaderboardEntry.Score}");
            }
        }
        #endregion
    }
}
