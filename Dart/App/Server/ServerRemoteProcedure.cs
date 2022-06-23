using Network;
using System.Net;
using Server.Infrastructure.BL;

namespace Server
{
    internal class ServerRemoteProcedure : RemoteProcedures
    {
        public override void OnConnected(string remoteEndPoint)
        {
            UserBL userBL = new UserBL();
            Guid id = userBL.Add(remoteEndPoint);

            IPEndPoint clientEndPoint = IPEndPoint.Parse(remoteEndPoint);

            Procedure procedure = new Procedure("Connected", new Parameter[] { new Parameter("userId", id.ToString()) });

            Program.ServerInstance.Send(clientEndPoint, procedure);
        }
    }
}
