using Network;
using System.Net;
using Server.Infrastructure.BL;
using Server.Domain.Core;

namespace Server.Application
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
            IResult<Guid> addUserResult = userBL.Add(string.Empty, remoteEndPoint);
            if (addUserResult.IsSuccessful)
            {
                GameSeasonBL gameSeasonBL = new GameSeasonBL(Program.ApplicationContext);
                IResult<Guid> gameSeasonResult = gameSeasonBL.Add(addUserResult.Message);
                if (gameSeasonResult.IsSuccessful)
                {
                    LeaderboadBL leaderboadBL = new LeaderboadBL(Program.ApplicationContext);
                    IResult addLeaderboardResult = leaderboadBL.Add(gameSeasonResult.Message);
                    if (addLeaderboardResult.IsSuccessful)
                    {
                        IPEndPoint clientEndPoint = IPEndPoint.Parse(remoteEndPoint);

                        Procedure procedure = new Procedure("Connected", new Parameter[] { new Parameter("userId", addUserResult.Message.ToString()) });

                        Program.ServerInstance.Send(clientEndPoint, procedure);
                    }
                }
            }
        }

        public void DartThrowed(Guid userId, int score)
        {
            LeaderboadBL leaderboadBL = new LeaderboadBL(Program.ApplicationContext);

            leaderboadBL.AddScore(userId, score);

            Program.ApplicationView.DisplayLeaderboard(leaderboadBL.GetAll());
        }
    }
}
