using System.Text;
using System.Text.Json.Nodes;

namespace Network
{
    public class Payload
    {
        #region Fields
        public static readonly int PAYLOAD_SIZE = Encoding.ASCII.GetBytes(new Payload(StateObject.BUFFER_SIZE).ToJson().ToJsonString()).Length;
        #endregion

        #region Properties
        public int Length { get; }

        public int RemainingLength { get; set; }
        #endregion

        #region Constructors
        public Payload(int length)
        {
            Length = length;
            RemainingLength = Length;
        }

        public Payload(JsonObject jsonObject)
        {
            Length = int.Parse(jsonObject[nameof(Length)].ToString());
            RemainingLength = Length;
        }
        #endregion

        #region Public Methods
        public JsonObject ToJson()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject[nameof(Length)] = Length.ToString("D5");
            return jsonObject;
        }
        #endregion
    }
}
