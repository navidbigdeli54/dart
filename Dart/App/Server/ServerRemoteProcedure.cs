using Network;
using System.Net;
using Server.Infrastructure.BL;
using Server.Domain;

namespace Server
{
    internal class ServerRemoteProcedure : RemoteProcedures
    {
        public override void OnConnected(string remoteEndPoint)
        {
            UserBL userBL = new UserBL();
            Guid userId = userBL.Add(remoteEndPoint);

            GameSeasonBL gameSeasonBL = new GameSeasonBL();
            Guid gameSeasonId = gameSeasonBL.Add(userId);

            LeaderboadBL leaderboadBL = new LeaderboadBL();
            leaderboadBL.Add(gameSeasonId);

            IPEndPoint clientEndPoint = IPEndPoint.Parse(remoteEndPoint);

            Procedure procedure = new Procedure("Connected", new Parameter[] { new Parameter("userId", userId.ToString()) });

            Program.ServerInstance.Send(clientEndPoint, procedure);
        }

        public void DartThrowed(string userId, int score)
        {
            LeaderboadBL leaderboadBL = new LeaderboadBL();

            leaderboadBL.AddScore(Guid.Parse(userId), score);

            Console.Clear();

            IReadOnlyList<LeaderBoardEntry> entries = leaderboadBL.GetAll();
            for (int i = 0; i < entries.Count; i++)
            {
                Console.WriteLine($"{entries[i].Rank}\t {entries[i].Score}");
            }
        }
    }
}
