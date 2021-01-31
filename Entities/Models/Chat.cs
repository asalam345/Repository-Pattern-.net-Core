using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class Chat
    {
        public long ChatId { get; set; }
        public string Message { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public bool IsDeleteFromReceiver { get; set; }
        public bool IsDeleteFromSender { get; set; }
    }
}
