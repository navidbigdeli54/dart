using Network;
using System.Net;
using Server.Infrastructure.BL;
using System.Collections.Generic;
using Server.Domain;

namespace Server
{
    public class Program
    {
        private static ServerInstance _serverInstance;

        public static ServerInstance ServerInstance
        {
            get
            {
                if (_serverInstance == null)
                {
                    IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress ipAddress = ipHostEntry.AddressList[0];
                    _serverInstance = new ServerInstance(new ServerRemoteProcedure(), ipAddress, 100);
                }

                return _serverInstance;
            }
        }

        static void Main()
        {
            ServerInstance.Start();

            LeaderboadBL leaderboadBL = new LeaderboadBL();

            while (true)
            {
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine($"# \t                 Id                  \tScore");

                IReadOnlyList<LeaderBoardEntry> leaderboardEntries = leaderboadBL.GetAll();
                for (int i = 0; i < leaderboardEntries.Count; ++i)
                {
                    var entry = leaderboardEntries[i];
                    Console.WriteLine($"{entry.Rank}\t {entry.Id} \t {entry.Score}");
                }
            }

            Console.Read();
        }
    }
}