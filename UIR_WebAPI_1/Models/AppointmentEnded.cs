using System;
using System.Collections.Generic;

namespace UIR_WebAPI_1.Models;

public partial class AppointmentEnded
{
    public int AppointmentId { get; set; }

    public int SpecialistId { get; set; }

    public int UserUirId { get; set; }

    public DateTime From1 { get; set; }

    public DateTime To1 { get; set; }

    public int? RatingId { get; set; }

    public virtual RatingScale? Rating { get; set; }

    public virtual Specialist? Specialist { get; set; }

    public virtual UserTable? UserUir { get; set; }
}
