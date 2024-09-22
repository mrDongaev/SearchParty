using System.Text.Json.Serialization;

namespace Common.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PositionName
    {
        Carry = 1,
        Midlane = 2,
        Offlane = 3,
        Roamer = 4,
        Support = 5,
    }
}
