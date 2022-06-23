using System.Net;
using System.Net.Sockets;
using System.Text;
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

        public async Task Send(byte[] buffer)
        {
            if (_socket.Connected == false)
            {
                Console.WriteLine("Socket is not connected!");
                return;
            }

            await _socket.SendAsync(buffer, SocketFlags.None);
        }
        #endregion

        #region Private Methods
        private void BeginRecieve(IAsyncResult asyncResult)
        {
            StateObject recieveStateObject = new StateObject(_socket);
            recieveStateObject.Socket.BeginReceive(recieveStateObject.Buffer, 0, recieveStateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(BeginRecieve), recieveStateObject);

            StateObject? stateObject = asyncResult.AsyncState as StateObject;
            if (stateObject != null)
            {
                int recievedLenght = stateObject.Socket.EndReceive(asyncResult);

                byte[] recivedBytes = stateObject.Buffer.Take(recievedLenght).ToArray();

                JsonObject jsonObject = JsonNode.Parse(Encoding.UTF8.GetString(recivedBytes)).AsObject();

                Procedure procedure = new Procedure(jsonObject);

                _remoteProcedures.Invoke(procedure);
            }
        }
        #endregion
    }
}
