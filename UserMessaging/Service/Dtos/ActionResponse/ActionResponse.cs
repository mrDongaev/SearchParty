using Library.Models.Enums;
using Service.Dtos.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.ActionResponse
{
    public class ActionResponse<T> where T : MessageDto
    {
        public ActionResponseStatus Status { get; set; }

        public string ActionMessage { get; set; } = string.Empty;

        public T? Message { get; set; }
    }
}
