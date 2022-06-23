using Network;
using System.Net;

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

            Console.Read();
        }
    }
}