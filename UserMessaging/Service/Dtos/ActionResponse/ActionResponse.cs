using Library.Models.Enums;
using Service.Dtos.Message;

namespace Service.Dtos.ActionResponse
{
    public class ActionResponse<T> where T : MessageDto
    {
        public ActionResponseStatus Status { get; set; }

        public string ActionMessage { get; set; } = string.Empty;

        public T? Message { get; set; }
    }
}
