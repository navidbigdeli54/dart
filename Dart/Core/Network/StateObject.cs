using System.Net.Sockets;

namespace Network
{
    internal class StateObject
    {
        public const int BUFFER_SIZE = 1024;

        public readonly byte[] Buffer = new byte[BUFFER_SIZE];

        public readonly Socket Socket;

        public StateObject(Socket socket)
        {
            Socket = socket;
        }
    }
}
