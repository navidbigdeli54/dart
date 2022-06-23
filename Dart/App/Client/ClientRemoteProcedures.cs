using Network;

namespace Client
{
    public class ClientRemoteProcedures : RemoteProcedures
    {
        private Game _game;

        public void Connected(string userId)
        {
            _game = new Game(Guid.Parse(userId));

            _game.Start();
        }

        public void UpdateLeaderboard(string strigifinedJson)
        {

        }
    }
}
