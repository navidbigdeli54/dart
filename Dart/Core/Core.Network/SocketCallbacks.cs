using System.Text;
using System.Net.Sockets;
using System.Text.Json.Nodes;

namespace Core.Network
{
    public abstract class SocketCallbacks
    {
        #region Fields
        protected IRemoteProcedures _remoteProcedures;
        #endregion

        #region Constructors
        public SocketCallbacks(IRemoteProcedures remoteProcedures)
        {
            _remoteProcedures = remoteProcedures;
        }
        #endregion

        #region Protected Methods
        protected void ReceiveCallback(IAsyncResult asyncResult)
        {
            StateObject? stateObject = asyncResult.AsyncState as StateObject;
            if (stateObject != null)
            {
                Socket socket = stateObject.Socket;

                if (socket.Connected)
                {
                    try
                    {
                        int recievedLength = socket.EndReceive(asyncResult);

                        if (recievedLength > 0)
                        {
                            stateObject.RecivedBytes.AddRange(stateObject.Buffer.Take(recievedLength));

                            if (stateObject.Payload == null)
                            {
                                string payloadContent = Encoding.ASCII.GetString(stateObject.RecivedBytes.ToArray(), 0, Payload.PAYLOAD_SIZE);
                                stateObject.Payload = new Payload(JsonNode.Parse(payloadContent).AsObject());
                                stateObject.RecivedBytes.RemoveRange(0, Payload.PAYLOAD_SIZE);

                                int remainingRecievedLength = recievedLength - Payload.PAYLOAD_SIZE;
                                int count = stateObject.Payload.RemainingLength < remainingRecievedLength ? stateObject.Payload.RemainingLength : remainingRecievedLength;
                                stateObject.Payload.RemainingLength -= count;
                            }
                            else
                            {
                                stateObject.Payload.RemainingLength -= stateObject.Payload.RemainingLength < recievedLength ? stateObject.Payload.RemainingLength : recievedLength;
                            }

                            if (stateObject.Payload.RemainingLength == 0)
                            {
                                string content = Encoding.ASCII.GetString(stateObject.RecivedBytes.ToArray(), 0, stateObject.Payload.Length);

                                JsonObject jsonObject = JsonNode.Parse(content).AsObject();
                                Procedure procedure = new Procedure(jsonObject);
                                _remoteProcedures.Invoke(procedure);

                                stateObject.RecivedBytes.RemoveRange(0, stateObject.Payload.Length);
                                stateObject.Payload = null;
                            }

                            socket.BeginReceive(stateObject.Buffer, 0, stateObject.Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), stateObject);
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }
            }
        }

        protected void Send(Socket client, Procedure procedure)
        {
            if (client.Connected)
            {
                try
                {
                    List<byte> buffer = new List<byte>(StateObject.BUFFER_SIZE);
                    byte[] serializedProcedureBytes = Encoding.ASCII.GetBytes(procedure.ToString());
                    Payload payload = new Payload(serializedProcedureBytes.Length);
                    buffer.AddRange(Encoding.ASCII.GetBytes(payload.ToJson().ToJsonString()));
                    buffer.AddRange(serializedProcedureBytes);
                    client.BeginSend(buffer.ToArray(), 0, buffer.Count, SocketFlags.None, new AsyncCallback(SendCallback), client);
                }
                catch (Exception exception)
                {
                    Console.Write(exception);
                }
            }
        }

        protected void SendCallback(IAsyncResult asyncResult)
        {
            Socket? socket = asyncResult.AsyncState as Socket;
            if (socket != null)
            {
                try
                {
                    socket.EndSend(asyncResult);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
        #endregion
    }
}
