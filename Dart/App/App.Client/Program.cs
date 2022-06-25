using System.Net;
using Core.Network;
using App.Client.Application;

namespace App.Client
{
    public class Program
    {
        private static ClientInstance _clientInstance;

        public static ClientInstance ClientInstance
        {
            get
            {
                if (_clientInstance == null)
                {
                    IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                    IPAddress ipAddress = ipHostEntry.AddressList[0];
                    _clientInstance = new ClientInstance(new ApplicatoinRemoteProcedures(), ipAddress, 100);
                }

                return _clientInstance;
            }
        }

        public static ApplicationView ApplicationView { get; } = new ApplicationView();

        static void Main()
        {
            ClientInstance.Connect();

            ApplicationView.DrawHeader();

            Console.Read();
        }
    }
}