using System.Text.Json.Serialization;

namespace Library.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActionResponseStatus
    {
        Success = 0,
        Failure,
    }
}
