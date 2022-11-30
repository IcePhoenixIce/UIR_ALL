using System;
using System.Collections.Generic;

namespace UIR_Service_B.Models;

public partial class Area
{
    public int AreaId { get; set; }

    public string AreaName { get; set; } = null!;

    public string AreaLocation { get; set; } = null!;

    public TimeSpan From1 { get; set; }

    public TimeSpan To1 { get; set; }

    public virtual ICollection<Room> Rooms { get; } = new List<Room>();
}
