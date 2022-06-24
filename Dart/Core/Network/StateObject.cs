using System.Net.Sockets;
using System.Text;

namespace Network
{
    public class StateObject
    {
        #region Fields
        public const int BUFFER_SIZE = 1024;
        #endregion

        #region Properties
        public byte[] Buffer { get; } = new byte[BUFFER_SIZE];

        public Socket Socket { get; }

        public StringBuilder StringBuilder { get; } = new StringBuilder();

        public Payload Payload { get; set; }
        #endregion

        public StateObject(Socket socket)
        {
            Socket = socket;
        }
    }
}
