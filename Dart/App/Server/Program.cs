using Network;
using System.Net;
using Server.Application;

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
                    _serverInstance = new ServerInstance(new ApplicationRemoteProcedures(), ipAddress, 100);
                }

                return _serverInstance;
            }
        }

        public static ApplicationContext ApplicationContext { get; } = new ApplicationContext();

        public static ApplicationView ApplicationView { get; } = new ApplicationView();

        static void Main()
        {
            ServerInstance.Start();

            ApplicationView.DrawHeader();

            Console.ReadLine();
        }
    }
}