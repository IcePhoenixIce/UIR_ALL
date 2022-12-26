namespace WebAppBlazor.Data.Models
{
    public class DataItem
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Text { get; set; }

        public DataItem() { this.Text = "Запись"; }
        public DataItem(DateTime Start, DateTime End)
        {
            this.Start = Start;
            this.End = End;
            this.Text = "Запись";
        }

        public static List<DataItem> dataItems(ICollection<SheduleTable> shedules)
        {
            var retList = new List<DataItem>();
            var dayOfWeek = DateTime.Today.DayOfWeek;
            foreach (var shedule in shedules)
            {
                DateTime datetime;
                if (shedule.WeekdayId == dayOfWeek)
                {
                    var datetime1 = DateTime.Now.AddMinutes(5);
                    datetime = DateTime.Today;
                    if (datetime1.TimeOfDay <= shedule.From1)
                    {
                        retList.Add(new DataItem(datetime + shedule.From1, datetime + shedule.LunchStart));
                        retList.Add(new DataItem(datetime + shedule.LunchEnd, datetime + shedule.To1));
                    }
                    else if (datetime1.TimeOfDay < shedule.LunchStart)
                    {
                        retList.Add(new DataItem(datetime1, datetime + shedule.LunchStart));
                        retList.Add(new DataItem(datetime + shedule.LunchEnd, datetime + shedule.To1));
                    }
                    else if (datetime1.TimeOfDay < shedule.LunchEnd)
                        retList.Add(new DataItem(datetime + shedule.LunchEnd, datetime + shedule.To1));
                    else if (datetime1.TimeOfDay < shedule.To1)
                        retList.Add(new DataItem(datetime1, datetime + shedule.To1));

                }
                else
                {
                    if (shedule.WeekdayId < dayOfWeek)
                    {
                        datetime = DateTime.Today.AddDays(7 - (int)dayOfWeek + (int)shedule.WeekdayId);
                    }
                    else
                    {
                        datetime = DateTime.Today.AddDays((int)shedule.WeekdayId - (int)dayOfWeek);
                    }
                    retList.Add(new DataItem(datetime + shedule.From1, datetime + shedule.LunchStart));
                    retList.Add(new DataItem(datetime + shedule.LunchEnd, datetime + shedule.To1));
                }
            }
            return retList;
        }

        public static List<DataItem> dataListXorAppointments(List<DataItem> dataItems, ICollection<AppointmentCurrent> appointments)
        {
            var retList = new List<DataItem>();
            foreach (var dataItem in dataItems)
            {
                var query = appointments.Where(a =>
                    (a.From1 == dataItem.Start && a.To1 < dataItem.End) ||
                    (a.From1 > dataItem.Start && a.To1 < dataItem.End) ||
                    (a.From1 > dataItem.Start && a.To1 == dataItem.End) ||
                    (a.From1 == dataItem.Start && a.To1 == dataItem.End));
                if (query.Any()) 
                {
                    query = (query.OrderBy(a => a.From1)).ToList();
                    DateTime start = dataItem.Start;
                    DateTime end  = dataItem.End;
                    foreach (var req in query) 
                    {
                        if(req.From1 != start)
                            retList.Add(new DataItem(start, req.From1));
                        start = req.To1;
                    }
                    if(start != end)
                        retList.Add(new DataItem(start, end));
                }
                else
                    retList.Add(dataItem);
            }
            return retList;
        }

        public static List<DataItem> dataListXorRecords(List<DataItem> dataItems, ICollection<RecordCurrent> appointments)
        {
            var retList = new List<DataItem>();
            foreach (var dataItem in dataItems)
            {
                var query = appointments.Where(a =>
                    (a.From1 <= dataItem.Start && a.To1 > dataItem.Start && a.To1 < dataItem.End) ||
                    (a.From1 > dataItem.Start && a.To1 < dataItem.End) ||
                    (a.From1 > dataItem.Start && a.From1 < dataItem.End && a.To1 >= dataItem.End) ||
                    (a.From1 == dataItem.Start && a.To1 == dataItem.End));
                if (query.Any())
                {
                    query = (query.OrderBy(a => a.From1)).ToList();
                    DateTime start = dataItem.Start;
                    DateTime end = dataItem.End;
                    foreach (var req in query)
                    {
                        if (req.From1 != start)
                            retList.Add(new DataItem(start, req.From1));
                        start = req.To1;
                    }
                    if (start != end)
                        retList.Add(new DataItem(start, end));
                }
                else
                    retList.Add(dataItem);
            }
            return retList;
        }

    }
}
