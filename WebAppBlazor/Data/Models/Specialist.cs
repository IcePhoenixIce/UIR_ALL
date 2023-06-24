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
}

public partial class SpecialistName : Specialist
{
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string MiddleName { get; set; } = null!;

    public SpecialistName(Specialist s)
    {
        this.SpecialistId = s.SpecialistId;
        this.Specialization = s.Specialization;
        this.Degree = s.Degree;
        this.Experience = s.Experience;
        this.AdditionalInfo = s.AdditionalInfo;
        this.AppointmentCurrents = s.AppointmentCurrents;
        this.AppointmentEndeds = s.AppointmentEndeds;
        this.SheduleTables = s.SheduleTables;
        this.SpecialistNavigation = s.SpecialistNavigation;
        this.PassGarmonyNavigation = s.PassGarmonyNavigation;
        this.Rooms = s.Rooms;
        if (s.SpecialistNavigation != null)
        {
            this.LastName = s.SpecialistNavigation.LastName;
            this.FirstName = s.SpecialistNavigation.FirstName;
            this.MiddleName = s.SpecialistNavigation.MiddleName;
        }
    }
}