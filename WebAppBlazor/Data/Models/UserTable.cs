using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebAppBlazor.Data.Models;

public partial class UserTable
{
    public int UserUirId { get; set; }
	[Required (ErrorMessage = "Необходимо указать имя")]
	public string LastName { get; set; } = null!;
	[Required (ErrorMessage = "Необходимо указать фамилию")]
	public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }
    [Phone (ErrorMessage = "Неправильный формат телефона")]
	[Required (ErrorMessage = "Необходимо указать телефон")]
	public string PhoneNumber { get; set; } = null!;
	[EmailAddress (ErrorMessage = "Неправильный формат почты")]
	[Required (ErrorMessage = "Необходимо указать почту")]
	public string EMail { get; set; } = null!;

    //public virtual ICollection<AppointmentCurrent> AppointmentCurrents { get; } = new List<AppointmentCurrent>();

    //public virtual ICollection<AppointmentEnded> AppointmentEndeds { get; } = new List<AppointmentEnded>();

    public virtual Pass? Pass { get; set; }

	//public virtual Specialist? Specialist { get; set; }

	public UserTable() { }
	public UserTable(UserTable user) 
	{
		this.UserUirId = user.UserUirId; 
		this.LastName = user.LastName;
		this.FirstName = user.FirstName;
		this.MiddleName = user.MiddleName;
		this.PhoneNumber = user.PhoneNumber;
		this.EMail = user.EMail;
		this.Pass = user.Pass;
	}
}
