using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Task_Managment_System.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public bool Read { get; set; }
        public NotificationType NotificationType { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}