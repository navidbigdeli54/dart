using System.Text.Json.Nodes;

namespace Domain.Model
{
    public struct ImmutableUserLeaderboardEntry
    {
        #region Properties
        public ImmutableUser User { get; }

        public ImmutableLeaderboardEntry LeaderboardEntry { get; }
        #endregion

        #region Constructors
        public ImmutableUserLeaderboardEntry(ImmutableUser user, ImmutableLeaderboardEntry leaderboardEntry)
        {
            User = user;
            LeaderboardEntry = leaderboardEntry;
        }

        public ImmutableUserLeaderboardEntry(JsonObject jsonObject)
        {
            User = new ImmutableUser(jsonObject["User"].AsObject());
            LeaderboardEntry = new ImmutableLeaderboardEntry(jsonObject["LeaderboardEntry"].AsObject());
        }
        #endregion

        #region Public Methods
        public JsonObject ToJson()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject["User"] = User.ToJson();
            jsonObject["LeaderboardEntry"] = LeaderboardEntry.ToJson();
            return jsonObject;
        }
        #endregion
    }
}
