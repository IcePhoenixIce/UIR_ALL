using System;
using System.Collections.Generic;

namespace WebAppBlazor.Data.Models
{
	public partial class WeekDay
	{
		public int WeekdayId { get; set; }

		public string WeekDay1 { get; set; } = null!;

		public virtual ICollection<SheduleTable> SheduleTables { get; } = new List<SheduleTable>();
	}
}


