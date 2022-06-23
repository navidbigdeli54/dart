using Network;
using System.Net;

namespace Client
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
                    _clientInstance = new ClientInstance(new ClientRemoteProcedures(), ipAddress, 100);
                }

                return _clientInstance;
            }
        }

        static void Main()
        {
            ClientInstance.Connect();

            Console.Read();
        }
    }
}