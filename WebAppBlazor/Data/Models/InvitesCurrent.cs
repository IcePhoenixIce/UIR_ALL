﻿using System;
using System.Collections.Generic;

namespace WebAppBlazor.Data.Models;

public partial class InvitesCurrent
{
    public int RecordId { get; set; }

    public int UserUirId { get; set; }

    public string? AdditionalInfo { get; set; }

    public virtual RecordCurrent? Record { get; set; }

    public virtual UserTable? UserUir { get; set; }
}
