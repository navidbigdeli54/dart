using System.Text.Json.Nodes;

namespace Core.Domain.Model
{
    public struct ImmutableScore
    {
        #region Properties
        public Guid Id { get; }

        public DateTime CreationDate { get; }

        public Guid GameSeasonId { get; }

        public int Point { get; }
        #endregion

        #region Constructors
        public ImmutableScore(Score score)
        {
            Id = score.Id;
            CreationDate = score.CreationDate;
            GameSeasonId = score.GameSeasonId;
            Point = score.Point;
        }

        public ImmutableScore(JsonObject jsonObject)
        {
            Id = Guid.Parse(jsonObject[nameof(Id)].ToString());
            CreationDate = DateTime.Parse(jsonObject[nameof(CreationDate)].ToString());
            GameSeasonId = Guid.Parse(jsonObject[nameof(GameSeasonId)].ToString());
            Point = int.Parse(nameof(Point));
        }
        #endregion

        #region Public Methods
        public JsonObject ToJson()
        {
            JsonObject jsonObject = new JsonObject();
            jsonObject[nameof(Id)] = Id.ToString();
            jsonObject[nameof(CreationDate)] = CreationDate.ToString();
            jsonObject[nameof(GameSeasonId)] = GameSeasonId.ToString();
            jsonObject[nameof(Point)] = Point.ToString();
            return jsonObject;
        }
        #endregion
    }
}
