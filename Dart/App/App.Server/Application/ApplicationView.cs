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
            GameSessionBL gameSessionBL = new GameSessionBL(Program.ApplicationContext);

            IReadOnlyList<ImmutableLeaderboard> allEntries = leaderboadBL.GetAll();
            for (int i = 0; i < allEntries.Count; i++)
            {
                ImmutableLeaderboard leaderboardEntry = allEntries[i];

                ImmutableGameSession gameSession = gameSessionBL.Get(leaderboardEntry.GameSessionId);

                ImmutableUser user = userBL.Get(gameSession.UserId);

                Console.WriteLine($"{leaderboardEntry.Rank}\t {user.Username}\t {leaderboardEntry.Score}");
            }
        }
        #endregion
    }
}
