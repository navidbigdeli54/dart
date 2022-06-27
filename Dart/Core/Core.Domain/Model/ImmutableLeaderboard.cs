using System.Text.Json.Nodes;

namespace Core.Domain.Model
{
    public struct ImmutableLeaderboard
    {
        #region Properties
        public Guid Id { get; }

        public Guid GameSessionId { get; }

        public int Score { get; }

        public int Rank { get; }

        public bool IsValid => Id != Guid.Empty;
        #endregion

        #region Properties
        public ImmutableLeaderboard(Leaderboard entry)
        {
            Id = entry.Id;
            Score = entry.Score;
            Rank = entry.Rank;
            GameSessionId = entry.GameSessionId;
        }

        public ImmutableLeaderboard(JsonObject jsonObject)
        {
            Id = Guid.Parse(jsonObject["Id"].ToString());
            GameSessionId = Guid.Parse(jsonObject["GameSessionId"].ToString());
            Score = int.Parse(jsonObject["Score"].ToString());
            Rank = int.Parse(jsonObject["Rank"].ToString());
        }
        #endregion

        #region Public Methos
        public JsonObject ToJson()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject["Id"] = Id;
            jsonObject["GameSessionId"] = GameSessionId;
            jsonObject["Score"] = Score;
            jsonObject["Rank"] = Rank;
            return jsonObject;
        }
        #endregion
    }
}
