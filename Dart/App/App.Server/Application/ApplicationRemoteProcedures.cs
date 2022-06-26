using System.Net;
using System.Text.Json.Nodes;
using App.Server.Infrastructure.BL;
using Core.Domain.Model;
using Core.Domain.Core;
using Core.Network;

namespace App.Server.Application
{
    internal class ApplicationRemoteProcedures : RemoteProcedures
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

            IReadOnlyList<ImmutableLeaderboard> previousTop3 = leaderboadBL.Get(3);

            leaderboadBL.AddScore(userId, score);

            Program.ApplicationView.DisplayLeaderboard(leaderboadBL.GetAll());

            IReadOnlyList<ImmutableLeaderboard> currentTop3 = leaderboadBL.Get(3);

            bool hasChanged = false;
            for (int i = 0; i < 3; ++i)
            {
                if (previousTop3[i].GameSeasonId != currentTop3[i].GameSeasonId || previousTop3[i].Score != currentTop3[i].Score)
                {
                    hasChanged = true;

                    break;
                }
            }

            if (hasChanged)
            {
                UserBL userBL = new UserBL(Program.ApplicationContext);
                ImmutableUser clientUser = userBL.Get(userId);
                if (clientUser.IsValid)
                {
                    List<ImmutableUserLeaderboard> leaderboardEntries = new List<ImmutableUserLeaderboard>();

                    GameSeasonBL gameSeasonBL = new GameSeasonBL(Program.ApplicationContext);

                    for (int i = 0; i < currentTop3.Count; ++i)
                    {
                        ImmutableLeaderboard leaderBoardEntry = currentTop3[i];

                        ImmutableGameSeason gameSeason = gameSeasonBL.Get(leaderBoardEntry.GameSeasonId);

                        ImmutableUser user = userBL.Get(gameSeason.UserId);

                        leaderboardEntries.Add(new ImmutableUserLeaderboard(user, leaderBoardEntry));
                    }

                    JsonObject jsonObject = new JsonObject();
                    JsonArray leaderboardArray = new JsonArray();
                    for (int i = 0; i < leaderboardEntries.Count; ++i)
                    {
                        leaderboardArray.Add(leaderboardEntries[i].ToJson());
                    }
                    jsonObject["Leaderboard"] = leaderboardArray;

                    IPEndPoint clientEndPoint = IPEndPoint.Parse(clientUser.EndPoint);

                    Procedure procedure = new Procedure("UpdateLeaderboard", new Parameter[] { new Parameter("leaderboard", jsonObject.ToJsonString()) });

                    Program.ServerInstance.Send(procedure);
                }
            }
        }
    }
}
