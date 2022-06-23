using Server.Domain;

namespace Server.Infrastructure.DAL
{
    public class GameSeasonDALProxy
    {
        private static readonly List<GameSeason> _gameSeasons = new List<GameSeason>();

        public void Add(GameSeason gameSeason)
        {
            gameSeason.Id = Guid.NewGuid();

            _gameSeasons.Add(gameSeason);
        }

        public GameSeason? Get(Guid gameSeasonId)
        {
            return _gameSeasons.Where(x => x.Id == gameSeasonId).SingleOrDefault();
        }

        public GameSeason? GetByUserId(Guid userId)
        {
            return _gameSeasons.Where(x => x.User.Id == userId).SingleOrDefault();
        }

        public void AddScore(Guid gameSeasonId, int score)
        {
            GameSeason? gameSeason = Get(gameSeasonId);
            if(gameSeason != null)
            {
                gameSeason.Scores.Add(score);
            }
        }
    }
}
