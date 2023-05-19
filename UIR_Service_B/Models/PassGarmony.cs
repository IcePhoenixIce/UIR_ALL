using System;
using System.Collections.Generic;

namespace UIR_Service_B.Models;

public partial class PassGarmony
{
    public int? UserUirId { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual UserTable? UserUir { get; set; }
}
