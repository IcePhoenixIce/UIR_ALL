using System;
using System.Collections.Generic;

namespace UIR_WebAPI_1.Models;

public partial class SheduleTable
{
    public int SpecialistId { get; set; }

    public int WeekdayId { get; set; }

    public TimeSpan From1 { get; set; }

    public TimeSpan To1 { get; set; }

    public TimeSpan LunchStart { get; set; }

    public TimeSpan LunchEnd { get; set; }

    public virtual Specialist? Specialist { get; set; }

    public virtual WeekDay? Weekday { get; set; }
}
