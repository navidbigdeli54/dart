using System.Text.Json.Nodes;

namespace Core.Domain.Model
{
    public struct ImmutableUserLeaderboard
    {
        #region Properties
        public ImmutableUser User { get; }

        public ImmutableLeaderboard LeaderboardEntry { get; }
        #endregion

        #region Constructors
        public ImmutableUserLeaderboard(ImmutableUser user, ImmutableLeaderboard leaderboardEntry)
        {
            User = user;
            LeaderboardEntry = leaderboardEntry;
        }

        public ImmutableUserLeaderboard(JsonObject jsonObject)
        {
            User = new ImmutableUser(jsonObject["User"].AsObject());
            LeaderboardEntry = new ImmutableLeaderboard(jsonObject["LeaderboardEntry"].AsObject());
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
