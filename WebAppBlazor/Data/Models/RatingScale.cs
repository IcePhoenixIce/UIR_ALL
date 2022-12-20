using System;
using System.Collections.Generic;

namespace WebAppBlazor.Data.Models 
{
	public partial class RatingScale
	{
		public int RatingId { get; set; }

		public int Rating { get; set; }

		public virtual ICollection<AppointmentEnded> AppointmentEndeds { get; } = new List<AppointmentEnded>();
	}

}

