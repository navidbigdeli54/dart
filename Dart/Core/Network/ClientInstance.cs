using System.Net;
using System.Text;
using System.Net.Sockets;

namespace Network
{
    public class ClientInstance : SocketCallback
    {
        #region Fields
        private IPEndPoint _remoteEndPoint;

        private Socket _socket;
        #endregion

        #region Properties
        public EndPoint? LocalEndPoint => _socket?.LocalEndPoint;

        public EndPoint? RemoteEndPoint => _socket?.RemoteEndPoint;
        #endregion

        #region Constructors
        public ClientInstance(IRemoteProcedures remoteProcedures, IPAddress serveIpAddress, int serverPort) : base(remoteProcedures)
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

                    StateObject recieveStateObject = new StateObject(_socket);
                    _socket.BeginReceive(recieveStateObject.Buffer, 0, recieveStateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), recieveStateObject);
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
                if (_socket.Connected)
                {
                    Send(_socket, procedure);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
        #endregion
    }
}
