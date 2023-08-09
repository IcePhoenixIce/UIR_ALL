using System;
using System.Collections.Generic;

namespace WebAppBlazor.Data.Models;

public partial class Specialist
{
    public int SpecialistId { get; set; }

    public string Specialization { get; set; } = null!;

    public string Degree { get; set; } = null!;

    public string Experience { get; set; } = null!;

    public string? AdditionalInfo { get; set; }

    public virtual ICollection<AppointmentCurrent>? AppointmentCurrents { get; set; }

    public virtual ICollection<AppointmentEnded>? AppointmentEndeds { get; set; }

    public virtual ICollection<SheduleTable>? SheduleTables { get; set; }

    public virtual UserTable? SpecialistNavigation { get; set; }

    public virtual PassGarmony? PassGarmonyNavigation { get; set; }

    public virtual ICollection<Room>? Rooms { get; set; }

    public string FirstName { get { return SpecialistNavigation.FirstName; } }
    public string LastName { get { return SpecialistNavigation.LastName; } }
    public string MiddleName { get { return SpecialistNavigation.MiddleName; } }
}