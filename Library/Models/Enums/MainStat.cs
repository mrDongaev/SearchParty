using System.Text.Json.Serialization;

namespace Library.Models.Enums
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
