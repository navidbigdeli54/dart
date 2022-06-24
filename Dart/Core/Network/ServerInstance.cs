using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Text.Json.Nodes;
using System.Collections.Concurrent;

namespace Network
{
    /*
     * TODO:
     * this should be disposable!
     */
    public class ServerInstance
    {
        #region Fields
        private readonly int _backlog;

        private readonly Socket _serverSocket;

        private readonly IPEndPoint _endpoint;

        private readonly IRemoteProcedures _remoteProcedures;

        private readonly ConcurrentBag<Socket> _clientSockets = new ConcurrentBag<Socket>();
        #endregion

        #region Constructors
        public ServerInstance(IRemoteProcedures remoteProcedures, IPAddress ipAddress, int port, int backlog = 1)
        {
            _remoteProcedures = remoteProcedures;

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
            Socket? client = _clientSockets.SingleOrDefault(x => x.RemoteEndPoint.Equals(endPoint));
            if (client != null)
            {
                Send(client, procedure);
            }
        }

        public void Send(Procedure procedure)
        {
            for (int i = 0; i < _clientSockets.Count; ++i)
            {
                foreach (Socket client in _clientSockets)
                {
                    Send(client, procedure);
                }
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

                _clientSockets.Add(clientSocket);
                _remoteProcedures.Invoke(new Procedure("OnConnected", new Parameter[] { new Parameter("remoteEndPoint", $"{clientSocket.RemoteEndPoint}") }));

                StateObject stateObject = new StateObject(clientSocket);
                clientSocket.BeginReceive(stateObject.Buffer, 0, stateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), stateObject);

                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), _serverSocket);
            }
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            StateObject? stateObject = asyncResult.AsyncState as StateObject;
            if (stateObject != null)
            {
                Socket clientSocket = stateObject.Socket;

                if (clientSocket.Connected)
                {
                    int recievedLenght = clientSocket.EndReceive(asyncResult);

                    if (recievedLenght > 0)
                    {
                        string payloadContent = Encoding.ASCII.GetString(stateObject.Buffer, 0, Payload.PAYLOAD_SIZE);
                        Payload payload = new Payload(JsonNode.Parse(payloadContent).AsObject());

                        string content = Encoding.ASCII.GetString(stateObject.Buffer, Payload.PAYLOAD_SIZE, payload.Lenght);
                        JsonObject jsonObject = JsonNode.Parse(content).AsObject();
                        Procedure procedure = new Procedure(jsonObject);
                        _remoteProcedures.Invoke(procedure);

                        StateObject nextStateObject = new StateObject(clientSocket);
                        clientSocket.BeginReceive(nextStateObject.Buffer, 0, nextStateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), nextStateObject);
                    }
                }
            }
        }

        private void Send(Socket client, Procedure procedure)
        {
            if (client.Connected)
            {
                List<byte> buffer = new List<byte>(StateObject.BUFFER_SIZE);
                byte[] serializedProcedureBytes = Encoding.ASCII.GetBytes(procedure.ToString());
                if (serializedProcedureBytes.Length <= StateObject.BUFFER_SIZE - Payload.PAYLOAD_SIZE)
                {
                    Payload payload = new Payload(serializedProcedureBytes.Length);
                    buffer.AddRange(Encoding.ASCII.GetBytes(payload.ToJson().ToJsonString()));
                    buffer.AddRange(serializedProcedureBytes);
                    client.BeginSend(buffer.ToArray(), 0, buffer.Count, SocketFlags.None, new AsyncCallback(SendCallback), client);
                }
                else
                {

                }
            }
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            Socket? clientSocket = asyncResult.AsyncState as Socket;
            if (clientSocket != null)
            {
                int byteSent = clientSocket.EndSend(asyncResult);
            }
        }
        #endregion
    }
}
