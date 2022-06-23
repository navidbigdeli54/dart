using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Text.Json.Nodes;

namespace Network
{
    public class ClientInstance
    {
        #region Fields
        private IPEndPoint _remoteEndPoint;

        private Socket _socket;

        private IRemoteProcedures _remoteProcedures;
        #endregion

        #region Properties
        public EndPoint? LocalEndPoint => _socket?.LocalEndPoint;

        public EndPoint? RemoteEndPoint => _socket?.RemoteEndPoint;
        #endregion

        #region Constructors
        public ClientInstance(IRemoteProcedures remoteProcedures, IPAddress serveIpAddress, int serverPort)
        {
            _remoteProcedures = remoteProcedures;

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

                    StateObject recieveStateObject = new StateObject(_socket);
                    _socket.BeginReceive(recieveStateObject.Buffer, 0, recieveStateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(BeginRecieve), recieveStateObject);
                }
                catch (Exception exception)
                {
                    ++attemp;
                    Console.Clear();
                    Console.WriteLine($"{exception.Message}, trying to connect for the {attemp}th time.");
                }
            }
        }

        public void Send(Procedure procedure)
        {
            try
            {
                if (_socket.Connected == false) throw new Exception("Client is not connected!");

                byte[] buffer = Encoding.ASCII.GetBytes(procedure.ToString());

                _socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), _socket);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        #endregion

        #region Private Methods
        private void BeginRecieve(IAsyncResult asyncResult)
        {
            StateObject? stateObject = asyncResult.AsyncState as StateObject;
            if (stateObject != null)
            {
                Socket clientSocket = stateObject.Socket;

                int recievedLenght = clientSocket.EndReceive(asyncResult);

                byte[] recivedBytes = stateObject.Buffer.Take(recievedLenght).ToArray();

                string content = Encoding.UTF8.GetString(recivedBytes);

                JsonObject jsonObject = JsonNode.Parse(content).AsObject();

                Procedure procedure = new Procedure(jsonObject);

                _remoteProcedures.Invoke(procedure);

                StateObject nextStateObject = new StateObject(clientSocket);
                _socket.BeginReceive(nextStateObject.Buffer, 0, nextStateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(BeginRecieve), nextStateObject);
            }
        }

        private void SendCallback(IAsyncResult asyncResult)
        {
            Socket? socket = asyncResult.AsyncState as Socket;
            if(socket != null)
            {
                socket.EndSend(asyncResult);
            }
        }
        #endregion
    }
}
