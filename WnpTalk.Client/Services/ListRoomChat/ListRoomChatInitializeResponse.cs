using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WnpTalk.Client.Services.ListRoomChat
{
    public class ListRoomChatInitializeResponse : BaseResponse
    {
        public User User { get; set; }
        public IEnumerable<User> UserRooms { get; set; }
        public IEnumerable<LastestChatRoomMessage> LastestChatRoomMessages { get; set; }
    }
}
