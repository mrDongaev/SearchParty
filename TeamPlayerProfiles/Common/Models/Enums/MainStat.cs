using System.Text.Json.Serialization;

namespace Common.Models.Enums
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
