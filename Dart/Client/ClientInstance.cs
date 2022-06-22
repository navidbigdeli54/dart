using System.Net;
using System.Text;
using System.Net.Sockets;

namespace Client
{
    public class ClientInstance
    {
        #region Fields
        private IPEndPoint _remoteEndPoint;

        private Socket _socket;
        #endregion

        #region Constructors
        public ClientInstance(IPAddress serveIpAddress, int serverPort)
        {
            _remoteEndPoint = new IPEndPoint(serveIpAddress, serverPort);

            _socket = new Socket(serveIpAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        #endregion

        #region Public Methods
        public void Connect()
        {
            int attemp = 0;

            while (_socket.Connected == false)
            {
                try
                {
                    _socket.Connect(_remoteEndPoint);
                }
                catch (Exception exception)
                {
                    ++attemp;
                    Console.WriteLine($"{exception.Message}, trying to connect for the {attemp}th time.");
                }
            }
        }
        #endregion
    }
}
