using System.Text.Json.Nodes;

namespace Core.Domain.Model
{
    public struct ImmutableGameSeason
    {
        #region Public Methods
        public Guid Id { get; }

        public DateTime CreationDate { get; }

        public Guid UserId { get; }

        public IReadOnlyList<ImmutableScore> Scores { get; }

        public bool IsValid => Id != Guid.Empty;
        #endregion

        #region Constructors
        public ImmutableGameSeason(GameSeason gameSeason, IReadOnlyList<ImmutableScore> scores)
        {
            Id = gameSeason.Id;
            CreationDate = gameSeason.CreationDate;
            UserId = gameSeason.UserId;
            Scores = scores;
        }

        public ImmutableGameSeason(JsonObject jsonObject)
        {
            Id = Guid.Parse(jsonObject["Id"].ToString());
            CreationDate = DateTime.Parse(jsonObject["CreationDate"].ToString());
            UserId = Guid.Parse(jsonObject["UserId"].ToString());

            List<ImmutableScore> scores = new List<ImmutableScore>();
            JsonArray scoreArray = jsonObject["Scores"].AsArray();
            for (int i = 0; i < scoreArray.Count; ++i)
            {
                scores.Add(new ImmutableScore(scoreArray[i].AsObject()));
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
                scoreArray.Add(Scores[i].ToJson());
            }
            jsonObject["Scores"] = scoreArray;
            return jsonObject;
        }
        #endregion
    }
}
