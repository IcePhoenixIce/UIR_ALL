namespace UIR_Service_B.Models
{
    public class booking
    {
        public int id { get; set; }
        public bool canCancel { get; set; }
        public bool canCancelOwner { get; set; }
        public bool canFullReturn { get; set; }
        public string beginAt { get; set; }
        public string lastCancelTime { get; set; }
        public int cabinetId { get; set; }
        public bool isOwner { get; set; }
        public Cabinet cabinet { get; set; }
        public string date { get; set; }
        public int hour { get; set; }
        public string minute { get; set; }
        public double sum { get; set; }
        public int cancelTime { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }

        public class Cabinet
        {
            public int id { get; set; }
            public Office office { get; set; }
            public string name { get; set; }
            public int weight { get; set; }
            public string createdAt { get; set; }
            public string updatedAt { get; set; }

            public class Office
            {
                public int id { get; set; }
                public string name { get; set; }
                public string address { get; set; }
                public Org org { get; set; }
                public string createdAt { get; set; }
                public string updatedAt { get; set; }

                public class Org
                {
                    public int id { get; set; }
                    public string name { get; set; }
                    public string createdAt { get; set; }
                    public string updatedAt { get; set; }
                }
            }
        }
    }
}

