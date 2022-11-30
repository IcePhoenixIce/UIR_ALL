using System;
using System.Collections.Generic;

namespace UIR_Service_B.Models;

public partial class RatingScale
{
    public int RatingId { get; set; }

    public int Rating { get; set; }

    public virtual ICollection<RecordEnded> RecordEndeds { get; } = new List<RecordEnded>();
}
