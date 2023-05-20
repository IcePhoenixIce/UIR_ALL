using System;
using System.Collections.Generic;

namespace UIR_WebAPI_1.Models;

public partial class Specialist
{
    public int SpecialistId { get; set; }

    public string Specialization { get; set; } = null!;

    public string Degree { get; set; } = null!;

    public string Experience { get; set; } = null!;

    public string? AdditionalInfo { get; set; }

    public virtual ICollection<AppointmentCurrent> AppointmentCurrents { get; } = new List<AppointmentCurrent>();

    public virtual ICollection<AppointmentEnded> AppointmentEndeds { get; } = new List<AppointmentEnded>();

    public virtual ICollection<SheduleTable> SheduleTables { get; } = new List<SheduleTable>();

    public virtual UserTable? SpecialistNavigation { get; set; }

    public virtual PassGarmony? PassDarmonyNavigation { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
