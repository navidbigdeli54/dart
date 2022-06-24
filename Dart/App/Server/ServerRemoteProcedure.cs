using Network;
using System.Net;
using Server.Infrastructure.BL;

namespace Server
{
    internal class ServerRemoteProcedure : RemoteProcedures
    {
        public override void OnConnected(string remoteEndPoint)
        {
            /*
             * TODO:
             * Client should send the username here!
             */
            UserBL userBL = new UserBL(Program.ApplicationContext);
            Guid userId = userBL.Add(string.Empty, remoteEndPoint);

            GameSeasonBL gameSeasonBL = new GameSeasonBL(Program.ApplicationContext);
            Guid gameSeasonId = gameSeasonBL.Add(userId);

            LeaderboadBL leaderboadBL = new LeaderboadBL(Program.ApplicationContext);
            leaderboadBL.Add(gameSeasonId);

            IPEndPoint clientEndPoint = IPEndPoint.Parse(remoteEndPoint);

            Procedure procedure = new Procedure("Connected", new Parameter[] { new Parameter("userId", userId.ToString()) });

            Program.ServerInstance.Send(clientEndPoint, procedure);
        }

        public void DartThrowed(Guid userId, int score)
        {
            LeaderboadBL leaderboadBL = new LeaderboadBL(Program.ApplicationContext);

            leaderboadBL.AddScore(userId, score);
        }
    }
}
