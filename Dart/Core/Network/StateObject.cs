using System.Net.Sockets;

namespace Network
{
    public class StateObject
    {
        #region Fields
        public const int BUFFER_SIZE = short.MaxValue;
        #endregion

        #region Properties
        public byte[] Buffer { get; } = new byte[BUFFER_SIZE];

        public Socket Socket { get; }
        #endregion

        public StateObject(Socket socket)
        {
            Socket = socket;
        }
    }
}
