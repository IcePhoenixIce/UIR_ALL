using System;
using System.Collections.Generic;

namespace WebAppBlazor.Data.Models;

public partial class Area
{
    public int AreaId { get; set; }

    public string AreaName { get; set; } = null!;

    public string AreaLocation { get; set; } = null!;

	public string Servis { get; set; }

	public string? AdditionalInfo { get; set; }

	public TimeSpan From1 { get; set; }

    public TimeSpan To1 { get; set; }

    public virtual ICollection<Room> Rooms { get; } = new List<Room>();
}
