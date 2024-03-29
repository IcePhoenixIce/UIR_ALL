﻿using System;
using System.Collections.Generic;

namespace UIR_Service_B.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? AdditionalInformation { get; set; }

    public int AreaId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public virtual Area? Area { get; set; }

    public virtual ICollection<RecordCurrent> RecordCurrents { get; } = new List<RecordCurrent>();

    public virtual ICollection<RecordEnded> RecordEndeds { get; } = new List<RecordEnded>();
}
