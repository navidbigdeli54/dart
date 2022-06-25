using System.Text.Json.Nodes;

namespace Domain.Model
{
    public struct ImmutableGameSeason
    {
        #region Public Methods
        public Guid Id { get; }

        public DateTime CreationDate { get; }

        public Guid UserId { get; }

        public IReadOnlyList<int> Scores { get; }

        public bool IsValid => Id != Guid.Empty;
        #endregion

        #region Constructors
        public ImmutableGameSeason(GameSeason gameSeason)
        {
            Id = gameSeason.Id;
            CreationDate = gameSeason.CreationDate;
            UserId = gameSeason.UserId;
            Scores = new List<int>(gameSeason.Scores);
        }

        public ImmutableGameSeason(JsonObject jsonObject)
        {
            Id = Guid.Parse(jsonObject["Id"].ToString());
            CreationDate = DateTime.Parse(jsonObject["CreationDate"].ToString());
            UserId = Guid.Parse(jsonObject["UserId"].ToString());

            List<int> scores = new List<int>();
            JsonArray scoreArray = jsonObject["Scores"].AsArray();
            for (int i = 0; i < scoreArray.Count; ++i)
            {
                scores.Add(int.Parse(scoreArray[i].ToString()));
            }
            Scores = scores;
        }
        #endregion

        #region Public Methos
        public JsonObject ToJson()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject["Id"] = Id;
            jsonObject["CreationDate"] = CreationDate;
            jsonObject["UserId"] = UserId;

            JsonArray scoreArray = new JsonArray();
            for (int i = 0; i < Scores.Count; ++i)
            {
                scoreArray.Add(Scores[i]);
            }
            jsonObject["Scores"] = scoreArray;
            return jsonObject;
        }
        #endregion
    }
}
