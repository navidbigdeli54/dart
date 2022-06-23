using Server.Domain;
using Server.Infrastructure.DAL;

namespace Server.Infrastructure.BL
{
    public class GameSeasonBL
    {
        public static readonly GameSeasonDALProxy _gameSeasonDAL = new GameSeasonDALProxy();

        public Guid Add(Guid userGuid)
        {
            UserDALProxy userDALProxy = new UserDALProxy();
            User? user = userDALProxy.Get(userGuid);

            if (user != null)
            {
                GameSeason gameSeason = new GameSeason
                {
                    CreationDate = DateTime.UtcNow,
                    User = user
                };

                _gameSeasonDAL.Add(gameSeason);

                return gameSeason.Id;
            }

            return Guid.Empty;
        }

        internal GameSeason? Get(Guid gameSeasonId)
        {
            return _gameSeasonDAL.Get(gameSeasonId);
        }
    }
}
