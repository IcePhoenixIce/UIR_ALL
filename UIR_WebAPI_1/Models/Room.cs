using System;
using System.Collections.Generic;

namespace UIR_WebAPI_1.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? AdditionalInformation { get; set; }

    public int? AreaId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public virtual Area? Area { get; set; }

    public virtual ICollection<Specialist> Specs { get; set; } = new List<Specialist>();

}
