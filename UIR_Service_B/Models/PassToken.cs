using System;
using System.Collections.Generic;

namespace UIR_Service_B.Models;

public partial class PassToken : Pass
{
    public PassToken(Pass pass) 
    {
        this.UserUirId=pass.UserUirId;
        this.UserLogin=pass.UserLogin;
        this.PasswordHash=pass.PasswordHash;
        this.UserUir=pass.UserUir;
    }
    public string Token { get; set; }
}
