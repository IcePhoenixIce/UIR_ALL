using System;
using System.Collections.Generic;

namespace UIR_WebAPI_1.Models;

public partial class UserTable
{
    public int UserUirId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string EMail { get; set; } = null!;

    public virtual ICollection<AppointmentCurrent> AppointmentCurrents { get; } = new List<AppointmentCurrent>();

    public virtual ICollection<AppointmentEnded> AppointmentEndeds { get; } = new List<AppointmentEnded>();

    public virtual Pass? Pass { get; set; }

    public virtual Specialist? Specialist { get; set; }
}
