using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WnpTalk.Client.Services.ChatRoomMessage
{
    public class ChatRoomMessageInitializeReponse : BaseResponse
    {
        public User RoomInfo { get; set; }
        public IEnumerable<Models.ChatRoomMessage> ChatRoomMessages { get; set; }
    }
}
