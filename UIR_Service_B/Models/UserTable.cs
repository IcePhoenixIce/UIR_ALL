using System;
using System.Collections.Generic;

namespace UIR_Service_B.Models;

public partial class UserTable
{
    public int UserUirId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string EMail { get; set; } = null!;

    public virtual ICollection<InvitesCurrent> InvitesCurrents { get; } = new List<InvitesCurrent>();

    public virtual ICollection<InvitesEnded> InvitesEndeds { get; } = new List<InvitesEnded>();

    public virtual Pass? Pass { get; set; }

    public virtual ICollection<RecordCurrent> RecordCurrents { get; } = new List<RecordCurrent>();

    public virtual ICollection<RecordEnded> RecordEndeds { get; } = new List<RecordEnded>();
}
