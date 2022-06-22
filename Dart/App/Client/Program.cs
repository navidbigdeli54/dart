using Network;
using System.Net;

namespace Client
{
    public class Program
    {
        static void Main()
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostEntry.AddressList[0];
            ClientInstance client = new Network.ClientInstance(ipAddress, 100);
            client.Connect();

            Console.Read();
        }
    }
}