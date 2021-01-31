using System;

namespace Entities.Models
{
    public class ChatMessage
    {
        public string Text { get; set; }
        public string ConnectionId { get; set; }
        public DateTime DateTime { get; set; }
        public string Time { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid SenderId { get; set; }
        public long ChatId { get; set; }
        public bool IsDeleteFromReceiver { get; set; }
        public bool IsDeleteFromSender { get; set; }
        public bool IsChnaged { get; set; }
    }
}
