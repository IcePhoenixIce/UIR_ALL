﻿namespace WebAppBlazor.Data.Models
{
    public class RecordService
    {
        public int SpecID { get; set; }
        public DateTime From1 { get; set; }
        public DateTime To1 { get; set; }
        public decimal Price { get; set; }
        public decimal PriceRoom { get; set; } = 0;
        public bool IsBooked { get; set; } = false;

        public bool IsChoosen { get; set; } = false;

        public string PriceStr 
        { 
            get { return "Цена: "+Price.ToString(); } 
            set { Price = Convert.ToDecimal(value); } 
        }
    }

	public class RecordShedule : RecordService
	{
        public SheduleTable sheduleTable { get; set; }
        public DateTime LunchEnd { get; set; }
        public DateTime LunchStart { get; set; }
		public string info { get; set; }
	}
}
