using System.Net.Sockets;

namespace Core.Network
{
    public class StateObject
    {
        #region Fields
        public const int BUFFER_SIZE = 1024;
        #endregion

        #region Properties
        public byte[] Buffer { get; } = new byte[BUFFER_SIZE];

        public Socket Socket { get; }

        public List<byte> RecivedBytes = new List<byte>();

        public Payload Payload { get; set; }
        #endregion

        public StateObject(Socket socket)
        {
            Socket = socket;
        }
    }
}
