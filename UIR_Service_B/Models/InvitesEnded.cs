using System;
using System.Collections.Generic;

namespace UIR_Service_B.Models;

public partial class InvitesEnded
{
    public int RecordId { get; set; }

    public int UserUirId { get; set; }

    public string? AdditionalInfo { get; set; }

    public virtual RecordEnded Record { get; set; } = null!;

    public virtual UserTable UserUir { get; set; } = null!;
}
