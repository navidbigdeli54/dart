using Network;

namespace Server
{
    internal class ServerRemoteProcedure : RemoteProcedures
    {
        public void ClientConnect(string clientName)
        {
            Console.WriteLine($"Client {clientName} has been connected!");
        }
    }
}
