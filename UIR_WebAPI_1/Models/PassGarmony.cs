using System;
using System.Collections.Generic;

namespace UIR_WebAPI_1.Models;

public partial class PassGarmony
{
    public int SpecialistID { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Specialist? Specialist { get; set; }
}
