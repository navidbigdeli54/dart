using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Text.Json.Nodes;
using System.Collections.Concurrent;

namespace Network
{
    public class ServerInstance
    {
        #region Fields
        private readonly IPEndPoint _endpoint;

        private readonly Socket _serverSocket;

        private readonly ConcurrentBag<Socket> _clientSockets = new ConcurrentBag<Socket>();

        private readonly IRemoteProcedures _remoteProcedures;
        #endregion

        #region Constructors
        public ServerInstance(IRemoteProcedures remoteProcedures, IPAddress ipAddress, int port, int blacklog = 1)
        {
            _remoteProcedures = remoteProcedures;

            _endpoint = new IPEndPoint(ipAddress, port);

            _serverSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Bind(_endpoint);
            _serverSocket.Listen(blacklog);
        }
        #endregion

        #region Public Methods
        public void Open()
        {
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), new StateObject(_serverSocket));
        }

        public void Send(IPEndPoint endPoint, Procedure procedure)
        {
            Socket? client = _clientSockets.Where(x => x.RemoteEndPoint.Equals(endPoint)).SingleOrDefault();
            if (client != null)
            {
                if (client.Connected)
                {
                    byte[] buffer = Encoding.ASCII.GetBytes(procedure.ToString());

                    StateObject sendStateObject = new StateObject(client);
                    client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(BeginRecieve), sendStateObject);
                }
            }
        }

        public void Send(Procedure procedure)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(procedure.ToString());

            for (int i = 0; i < _clientSockets.Count; ++i)
            {
                foreach(Socket client in _clientSockets)
                {
                    if (client.Connected)
                    {
                        StateObject sendStateObject = new StateObject(client);
                        client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(BeginRecieve), sendStateObject);
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        private void AcceptCallback(IAsyncResult asyncResult)
        {
            StateObject nextStateObject = new StateObject(_serverSocket);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), nextStateObject);

            Socket clientSocket = _serverSocket.EndAccept(asyncResult);

            _clientSockets.Add(clientSocket);
            _remoteProcedures.Invoke(new Procedure("OnConnected", new Parameter[] { new Parameter("remoteEndPoint", $"{clientSocket.RemoteEndPoint}") }));

            StateObject stateObject = new StateObject(clientSocket);
            clientSocket.BeginReceive(stateObject.Buffer, 0, stateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), stateObject);
        }

        private void ReceiveCallback(IAsyncResult asyncResult)
        {
            StateObject? stateObject = asyncResult.AsyncState as StateObject;
            if (stateObject != null)
            {
                StateObject nextStateObject = new StateObject(stateObject.Socket);
                stateObject.Socket.BeginReceive(nextStateObject.Buffer, 0, nextStateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), nextStateObject);

                int recievedLenght = stateObject.Socket.EndReceive(asyncResult);

                byte[] recivedBytes = stateObject.Buffer.Take(recievedLenght).ToArray();

                string strigifiedJson = Encoding.ASCII.GetString(recivedBytes);

                JsonObject jsonObject = JsonNode.Parse(strigifiedJson).AsObject();

                Procedure procedure = new Procedure(jsonObject);

                _remoteProcedures.Invoke(procedure);
            }
        }

        private void BeginRecieve(IAsyncResult asyncResult)
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
    }
}
