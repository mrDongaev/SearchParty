using System.Text.Json.Serialization;

namespace Common.Enums
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MainStat
    {
        Agility,
        Intelligence,
        Strength,
        Universal
    }
}
