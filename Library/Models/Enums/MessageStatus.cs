using System.Text.Json.Serialization;

namespace Library.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MessageStatus
    {
        Pending,
        Accepted,
        Rejected,
        Expired,
    }
}
