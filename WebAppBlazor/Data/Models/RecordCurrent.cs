using System;
using System.Collections.Generic;
using WebAppBlazor.Data.Models;

namespace WebAppBlazor.Data.Models
{
    public partial class RecordCurrent
    {
        public int RecordId { get; set; }

        public int RoomId { get; set; }

        public int UserUirId { get; set; }

        public DateTime From1 { get; set; }

        public DateTime To1 { get; set; }

        public virtual ICollection<InvitesCurrent>? InvitesCurrent { get; set; }

        public virtual Room? Room { get; set; }

        public virtual UserTable? UserUir { get; set; }
    }
}