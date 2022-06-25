using Core.Domain.Model;
using Core.Network;
using System.Text.Json.Nodes;

namespace App.Client.Application
{
    public class ApplicatoinRemoteProcedures : RemoteProcedures
    {
        private Game _game;

        public void Connected(string userId)
        {
            _game = new Game(Guid.Parse(userId));

            _game.Start();
        }

        public void UpdateLeaderboard(string leaderboard)
        {
            JsonObject jsonObject = JsonNode.Parse(leaderboard).AsObject();

            List<ImmutableUserLeaderboardEntry> entries = new List<ImmutableUserLeaderboardEntry>();
            JsonArray entriesArray = jsonObject["Leaderboard"].AsArray();
            for (int i = 0; i < entriesArray.Count; ++i)
            {
                entries.Add(new ImmutableUserLeaderboardEntry(entriesArray[i].AsObject()));
            }

            Program.ApplicationView.DisplayLeaderboard(entries);
        }
    }
}
