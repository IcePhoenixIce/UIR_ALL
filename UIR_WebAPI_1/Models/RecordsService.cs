namespace UIR_WebAPI_1.Models
{
    public class RecordService
    {
        public int SpecID { get; set; }
        public DateTime From1 { get; set; }
        public DateTime To1 { get; set; }
        public decimal Price { get; set; }
        public bool IsBooked { get; set; } = false;
    }
}
