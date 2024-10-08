using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LineBotMessage.Dtos
{
    public class ReplyMessageRequestDto<T>
    {
        public string  ReplyToken { get; set; }
        public List<T> Messages { get; set; }
        public bool? NotificationDisabled { get; set; }
    }
}