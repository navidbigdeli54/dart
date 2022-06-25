using System.Text.Json.Nodes;

namespace Domain.Model
{
    public struct ImmutableUser
    {
        #region Properties
        public Guid Id { get; }

        public string Username { get; }

        public string EndPoint { get; }

        public bool IsValid => Id != Guid.Empty;
        #endregion

        #region Constructors
        public ImmutableUser(User user)
        {
            Id = user.Id;
            Username = user.Username;
            EndPoint = user.EndPoint;
        }

        public ImmutableUser(JsonObject jsonObject)
        {
            Id = Guid.Parse(jsonObject["Id"].ToString());
            Username = jsonObject["Username"].ToString();
            EndPoint = jsonObject["EndPoint"].ToString();
        }
        #endregion

        #region Public Methods
        public JsonObject ToJson()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject["Id"] = Id;
            jsonObject["Username"] = Username;
            jsonObject["EndPoint"] = EndPoint;
            return jsonObject;
        }
        #endregion
    }
}
