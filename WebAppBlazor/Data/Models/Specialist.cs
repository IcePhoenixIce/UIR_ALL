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

    public virtual ICollection<AppointmentCurrent> AppointmentCurrents { get; } = new List<AppointmentCurrent>();

    public virtual ICollection<AppointmentEnded> AppointmentEndeds { get; } = new List<AppointmentEnded>();

    public virtual ICollection<SheduleTable> SheduleTables { get; } = new List<SheduleTable>();

    public virtual UserTable? SpecialistNavigation { get; set; }

    public virtual PassGarmony? PassDarmonyNavigation { get; set; }

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
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
        if (s.SpecialistNavigation != null)
        {
            this.LastName = s.SpecialistNavigation.LastName;
            this.FirstName = s.SpecialistNavigation.FirstName;
            this.MiddleName = s.SpecialistNavigation.MiddleName;
        }
        if(s.Rooms != null)
            this.Rooms = s.Rooms;
    }
}