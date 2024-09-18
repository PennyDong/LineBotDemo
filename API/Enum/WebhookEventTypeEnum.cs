using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Enum
{
    public class WebhookEventTypeEnum
    {
        public const string Message="message";
        public const string Unsend = "unsend";
        public const string Follow = "follow";
        public const string Unfollow = "unfollow";
        public const string Join = "join";
        public const string Leave = "leave";
         public const string MemberJoined = "memberJoined";
        public const string MemberLeft = "memberLeft";
        public const string Postback = "postback";
        public const string VideoPlayComplete = "videoPlayComplete";        
    }
}