using Network;
using System.Net;

namespace Server
{
    public class Program
    {
        static void Main()
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostEntry.AddressList[0];
            ServerInstance serverInstance = new Network.ServerInstance(ipAddress, 100);
            serverInstance.Listen();

            Console.Read();
        }
    }
}