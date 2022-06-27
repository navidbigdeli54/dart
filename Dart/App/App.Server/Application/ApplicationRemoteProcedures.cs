﻿using System.Net;
using Core.Network;
using Core.Domain.Core;
using Core.Domain.Model;
using System.Text.Json.Nodes;
using Core.BL;

namespace App.Server.Application
{
    public class ApplicationRemoteProcedures : RemoteProcedures
    {
        public override void OnConnected(string remoteEndPoint)
        {
            Procedure procedure = new Procedure("Connected", new Parameter[0]);

            Program.ServerInstance.Send(IPEndPoint.Parse(remoteEndPoint), procedure);
        }

        public void RegisterUser(string username, string remoteEndPoint)
        {
            UserBL userBL = new UserBL(Program.ApplicationContext);
            LeaderboadBL leaderboadBL = new LeaderboadBL(Program.ApplicationContext);
            GameSeasonBL gameSeasonBL = new GameSeasonBL(Program.ApplicationContext);

            IPEndPoint clientEndPoint = IPEndPoint.Parse(remoteEndPoint);

            IResult<Guid> addUserResult = userBL.Add(username, remoteEndPoint);
            if (addUserResult.IsSuccessful)
            {
                IResult<Guid> gameSeasonResult = gameSeasonBL.Add(addUserResult.Message);
                if (gameSeasonResult.IsSuccessful)
                {
                    IResult addLeaderboardResult = leaderboadBL.Add(gameSeasonResult.Message);
                    if (addLeaderboardResult.IsSuccessful)
                    {
                        Procedure userRegisteredProcedure = new Procedure("UserRegistered", new Parameter[] { new Parameter("userId", addUserResult.Message.ToString()) });

                        Program.ServerInstance.Send(clientEndPoint, userRegisteredProcedure);
                    }
                }
            }

            List<ImmutableUserLeaderboard> leaderboardEntries = new List<ImmutableUserLeaderboard>();
            IReadOnlyList<ImmutableLeaderboard> top3 = leaderboadBL.Get(3);
            for (int i = 0; i < top3.Count; ++i)
            {
                ImmutableLeaderboard leaderBoardEntry = top3[i];

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

            Procedure procedure = new Procedure("UpdateLeaderboard", new Parameter[] { new Parameter("leaderboard", jsonObject.ToJsonString()) });

            Program.ServerInstance.Send(clientEndPoint, procedure);
        }

        public void DartThrowed(Guid userId, int score)
        {
            LeaderboadBL leaderboadBL = new LeaderboadBL(Program.ApplicationContext);

            IReadOnlyList<ImmutableLeaderboard> previousTop3 = leaderboadBL.Get(3);

            leaderboadBL.AddScore(userId, score);

            Program.ApplicationView.DisplayLeaderboard();

            UpdateClient(previousTop3);
        }

        private static void UpdateClient(IReadOnlyList<ImmutableLeaderboard> previousTop3)
        {
            UserBL userBL = new UserBL(Program.ApplicationContext);
            GameSeasonBL gameSeasonBL = new GameSeasonBL(Program.ApplicationContext);
            LeaderboadBL leaderboadBL = new LeaderboadBL(Program.ApplicationContext);

            IReadOnlyList<ImmutableLeaderboard> currentTop3 = leaderboadBL.GetAll().Take(3).ToList();
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
                List<ImmutableUserLeaderboard> leaderboardEntries = new List<ImmutableUserLeaderboard>();

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

                Procedure procedure = new Procedure("UpdateLeaderboard", new Parameter[] { new Parameter("leaderboard", jsonObject.ToJsonString()) });

                Program.ServerInstance.Send(procedure);
            }
        }
    }
}
