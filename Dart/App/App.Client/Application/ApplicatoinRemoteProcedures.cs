using Core.Domain.Model;
using Core.Network;
using System.Text.Json.Nodes;

namespace App.Client.Application
{
    public class ApplicatoinRemoteProcedures : RemoteProcedures
    {
        private Game _game;

        public void Connected()
        {
            string[] names = { "Navid", "Zahra", "Shadi", "Hasan", "Negin", "Mohammad", "Laleh" };

            Procedure procedure = new Procedure("RegisterUser", new Parameter[] {
                new Parameter("username", names[Random.Shared.Next(0, names.Length)]),
                new Parameter("remoteEndPoint", Program.ClientInstance.LocalEndPoint.ToString())
            });

            Program.ClientInstance.Send(procedure);
        }

        public void UserRegistered(string userId)
        {
            _game = new Game(Guid.Parse(userId));

            _game.Start();
        }

        public void UpdateLeaderboard(string leaderboard)
        {
            JsonObject jsonObject = JsonNode.Parse(leaderboard).AsObject();

            List<ImmutableUserLeaderboard> entries = new List<ImmutableUserLeaderboard>();
            JsonArray entriesArray = jsonObject["Leaderboard"].AsArray();
            for (int i = 0; i < entriesArray.Count; ++i)
            {
                entries.Add(new ImmutableUserLeaderboard(entriesArray[i].AsObject()));
            }

            Program.ApplicationView.DisplayLeaderboard(entries);
        }
    }
}
