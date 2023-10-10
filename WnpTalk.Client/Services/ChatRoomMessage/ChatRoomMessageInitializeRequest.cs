using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WnpTalk.Client.Services.ChatRoomMessage
{
    public class ChatRoomMessageInitializeRequest
    {
        public int FromUserId { get; set; }
        public int ToRoomId { get; set; }
    }
}
