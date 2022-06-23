using Server.Domain;
using Server.Infrastructure.DAL;

namespace Server.Infrastructure.BL
{
    public class LeaderboadBL
    {
        private readonly LeaderboardDALProxy _leaderboardDALProxy = new LeaderboardDALProxy();

        public Guid Add(Guid gameSeasonId)
        {
            GameSeasonBL gameSeasonBL = new GameSeasonBL();
            GameSeason? gameSeason = gameSeasonBL.Get(gameSeasonId);
            if (gameSeason != null)
            {
                LeaderBoardEntry leaderBoardEntry = new LeaderBoardEntry
                {
                    GameSeason = gameSeason
                };

                _leaderboardDALProxy.Add(leaderBoardEntry);

                return leaderBoardEntry.Id;
            }

            return Guid.Empty;
        }

        public void AddScore(Guid userId, int score)
        {
            GameSeasonBL gameSeasonBL = new GameSeasonBL();
            GameSeason? gameSeason = gameSeasonBL.AddScore(userId, score);
            if (gameSeason != null)
            {
                _leaderboardDALProxy.UpdateScore(gameSeason.Id, gameSeason.Scores.Sum());
            }
        }

        public IReadOnlyList<LeaderBoardEntry> GetAll()
        {
            return _leaderboardDALProxy.GetAll();
        }
    }
}
