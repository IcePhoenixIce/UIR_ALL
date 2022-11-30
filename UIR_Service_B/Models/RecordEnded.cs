﻿using System;
using System.Collections.Generic;

namespace UIR_Service_B.Models;

public partial class RecordEnded
{
    public int RecordId { get; set; }

    public int RoomId { get; set; }

    public int UserUirId { get; set; }

    public DateTime From1 { get; set; }

    public DateTime To1 { get; set; }

    public int? RatingId { get; set; }

    public virtual InvitesEnded? InvitesEnded { get; set; }

    public virtual RatingScale? Rating { get; set; }

    public virtual Room Room { get; set; } = null!;

    public virtual UserTable UserUir { get; set; } = null!;
}