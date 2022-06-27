using System.Text.Json.Nodes;

namespace Core.Domain.Model
{
    public struct ImmutableScore
    {
        #region Properties
        public Guid Id { get; }

        public DateTime CreationDate { get; }

        public Guid GameSessionId { get; }

        public int Point { get; }
        #endregion

        #region Constructors
        public ImmutableScore(Score score)
        {
            Id = score.Id;
            CreationDate = score.CreationDate;
            GameSessionId = score.GameSessionId;
            Point = score.Point;
        }

        public ImmutableScore(JsonObject jsonObject)
        {
            Id = Guid.Parse(jsonObject[nameof(Id)].ToString());
            CreationDate = DateTime.Parse(jsonObject[nameof(CreationDate)].ToString());
            GameSessionId = Guid.Parse(jsonObject[nameof(GameSessionId)].ToString());
            Point = int.Parse(nameof(Point));
        }
        #endregion

        #region Public Methods
        public JsonObject ToJson()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject[nameof(Id)] = Id.ToString();
            jsonObject[nameof(CreationDate)] = CreationDate.ToString();
            jsonObject[nameof(GameSessionId)] = GameSessionId.ToString();
            jsonObject[nameof(Point)] = Point.ToString();
            return jsonObject;
        }
        #endregion
    }
}
