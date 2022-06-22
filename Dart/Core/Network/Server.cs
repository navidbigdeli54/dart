using System.Net;
using System.Text;
using System.Net.Sockets;

namespace Network
{
    public class ServerInstance
    {
        #region Fields
        private readonly IPEndPoint _endpoint;

        private readonly Socket _serverSocket;

        private readonly List<Socket> _clientSockets = new List<Socket>();
        #endregion

        #region Constructors
        public ServerInstance(IPAddress ipAddress, int port, int blacklog = 1)
        {
            _endpoint = new IPEndPoint(ipAddress, port);

            _serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(_endpoint);
            _serverSocket.Listen(blacklog);
        }
        #endregion

        #region Public Methods
        public void Listen()
        {
            Console.WriteLine("Starting server ...");
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), new StateObject(_serverSocket));
        }
        #endregion

        #region Private Methods
        private void AcceptCallback(IAsyncResult asyncResult)
        {
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), asyncResult.AsyncState);

            Socket clientSocket = _serverSocket.EndAccept(asyncResult);

            _clientSockets.Add(clientSocket);

            StateObject stateObject = new StateObject(clientSocket);
            clientSocket.BeginReceive(stateObject.Buffer, 0, stateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), stateObject);

            Console.WriteLine("client connected!");
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            StateObject? stateObject = asyncResult.AsyncState as StateObject;
            if (stateObject != null)
            {
                int recievedLenght = stateObject.Socket.EndReceive(asyncResult);

                byte[] recivedBytes = stateObject.Buffer.Take(recievedLenght).ToArray();

                Console.WriteLine(Encoding.ASCII.GetString(recivedBytes));
            }
        }
        #endregion

        #region Nested Types
        private class StateObject
        {
            public const int BUFFER_SIZE = 1024;

            public readonly byte[] Buffer = new byte[BUFFER_SIZE];

            public readonly Socket Socket;

            public StateObject(Socket socket)
            {
                Socket = socket;
            }
        }
        #endregion
    }
}
