using System;
using System.Collections.Generic;

namespace WebAppBlazor.Data.Models;

public partial class RecordEnded
{
    public int RecordId { get; set; }

    public int RoomId { get; set; }

    public int UserUirId { get; set; }

    public DateTime From1 { get; set; }

    public DateTime To1 { get; set; }

    public int? RatingId { get; set; }

	public string? ServiceRecord { get; set; }

	public virtual ICollection<InvitesEnded>? InvitesEndeds { get; set; }

    public virtual RatingScale? Rating { get; set; }

    public virtual Room? Room { get; set; }

    public virtual UserTable? UserUir { get; set; }
}
