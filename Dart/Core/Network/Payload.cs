using System.Text;
using System.Text.Json.Nodes;

namespace Network
{
    public struct Payload
    {
        #region Fields
        public static readonly int PAYLOAD_SIZE = Encoding.ASCII.GetBytes(new Payload(StateObject.BUFFER_SIZE).ToJson().ToJsonString()).Length; 
        #endregion

        #region Properties
        public int Lenght { get; }
        #endregion

        #region Constructors
        public Payload(int lenght)
        {
            Lenght = lenght;
        }

        public Payload(JsonObject jsonObject)
        {
            Lenght = int.Parse(jsonObject[nameof(Lenght)].ToString());
        }
        #endregion

        #region Public Methods
        public JsonObject ToJson()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject[nameof(Lenght)] = Lenght.ToString("D5");
            return jsonObject;
        }
        #endregion
    }
}
