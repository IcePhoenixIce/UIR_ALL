using System;
using System.Collections.Generic;

namespace WebAppBlazor.Data.Models
{
	public partial class AppointmentCurrent
	{
		public int AppointmentId { get; set; }

		public int SpecialistId { get; set; }

		public int UserUirId { get; set; }

		public DateTime From1 { get; set; }

		public DateTime To1 { get; set; }

		public virtual Specialist? Specialist { get; set; }

		public virtual UserTable? UserUir { get; set; }
	}
}


