using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WnpTalk.Client.Models
{
    public class ChatRoomMessage
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public int ToRoomId { get; set; }
        public string Content { get; set; }
        public DateTime SendDateTime { get; set; }
        public bool IsRead { get; set; }
    }
}
