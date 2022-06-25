using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace Core.Network
{
    /*
     * TODO:
     * this should be disposable!
     */
    public class ServerInstance : SocketCallbacks
    {
        #region Fields
        private readonly int _backlog;

        private readonly Socket _serverSocket;

        private readonly IPEndPoint _endpoint;

        private readonly ConcurrentDictionary<EndPoint, Socket> _clientSockets = new ConcurrentDictionary<EndPoint, Socket>();
        #endregion

        #region Constructors
        public ServerInstance(IRemoteProcedures remoteProcedures, IPAddress ipAddress, int port, int backlog = 1) : base(remoteProcedures)
        {
            _backlog = backlog;

            _endpoint = new IPEndPoint(ipAddress, port);

            _serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            try
            {
                _serverSocket.Bind(_endpoint);
                _serverSocket.Listen(_backlog);

                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), _serverSocket);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        public void Send(IPEndPoint endPoint, Procedure procedure)
        {
            if (_clientSockets.TryGetValue(endPoint, out Socket? client))
            {
                Send(client, procedure);
            }
        }

        public void Send(Procedure procedure)
        {
            foreach (KeyValuePair<EndPoint, Socket> pair in _clientSockets)
            {
                Socket client = pair.Value;
                Send(client, procedure);
            }
        }
        #endregion

        #region Private Methods
        private void AcceptCallback(IAsyncResult asyncResult)
        {
            Socket? serverSocket = asyncResult.AsyncState as Socket;
            if (serverSocket != null)
            {
                Socket clientSocket = _serverSocket.EndAccept(asyncResult);

                _clientSockets.AddOrUpdate(clientSocket.RemoteEndPoint, clientSocket, (remoteEndPoint, oldSocket) =>
                {
                    oldSocket.Close();

                    return clientSocket;
                });
                _remoteProcedures.Invoke(new Procedure("OnConnected", new Parameter[] { new Parameter("remoteEndPoint", $"{clientSocket.RemoteEndPoint}") }));

                StateObject stateObject = new StateObject(clientSocket);
                clientSocket.BeginReceive(stateObject.Buffer, 0, stateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), stateObject);

                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), _serverSocket);
            }
        }
        #endregion
    }
}
