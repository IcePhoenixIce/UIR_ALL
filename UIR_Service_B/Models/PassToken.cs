using System;
using System.Collections.Generic;

namespace UIR_Service_B.Models;

public partial class PassToken
{
    public PassToken(Pass pass) 
    {
        this.UserUirId=pass.UserUirId;
        this.UserLogin=pass.UserLogin;
        this.PasswordHash=pass.PasswordHash;
        this.UserUir=pass.UserUir;
    }
    public string Token { get; set; }
    public int UserUirId { get; set; }

    public string UserLogin { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual UserTable? UserUir { get; set; }
}
