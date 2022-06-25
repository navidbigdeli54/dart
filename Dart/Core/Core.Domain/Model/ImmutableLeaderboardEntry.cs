using System.Text.Json.Nodes;

namespace Core.Domain.Model
{
    public struct ImmutableLeaderboardEntry
    {
        #region Properties
        public Guid Id { get; }

        public Guid GameSeasonId { get; }

        public int Score { get; }

        public int Rank { get; }

        public bool IsValid => Id != Guid.Empty;
        #endregion

        #region Properties
        public ImmutableLeaderboardEntry(LeaderboardEntry entry)
        {
            Id = entry.Id;
            Score = entry.Score;
            Rank = entry.Rank;
            GameSeasonId = entry.GameSeasonId;
        }

        public ImmutableLeaderboardEntry(JsonObject jsonObject)
        {
            Id = Guid.Parse(jsonObject["Id"].ToString());
            GameSeasonId = Guid.Parse(jsonObject["GameSeasonId"].ToString());
            Score = int.Parse(jsonObject["Score"].ToString());
            Rank = int.Parse(jsonObject["Rank"].ToString());
        }
        #endregion

        #region Public Methos
        public JsonObject ToJson()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject["Id"] = Id;
            jsonObject["GameSeasonId"] = GameSeasonId;
            jsonObject["Score"] = Score;
            jsonObject["Rank"] = Rank;
            return jsonObject;
        }
        #endregion
    }
}
