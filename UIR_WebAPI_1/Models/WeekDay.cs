using System;
using System.Collections.Generic;

namespace UIR_WebAPI_1.Models;

public partial class WeekDay
{
    public int WeekdayId { get; set; }

    public string WeekDay1 { get; set; } = null!;

    public virtual ICollection<SheduleTable> SheduleTables { get; } = new List<SheduleTable>();
}
