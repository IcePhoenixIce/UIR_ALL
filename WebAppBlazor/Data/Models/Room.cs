using System;
using System.Collections.Generic;

namespace WebAppBlazor.Data.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? AdditionalInformation { get; set; }

    public int? AreaId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public virtual Area? Area { get; set; }

    public virtual ICollection<Specialist> Specs { get; set; } = new List<Specialist>();

    public virtual ICollection<RecordCurrent> RecordCurrents { get; } = new List<RecordCurrent>();

    public virtual ICollection<RecordEnded> RecordEndeds { get; } = new List<RecordEnded>();

    public override bool Equals(object obj)
    {
        // Если объект null, то он не равен текущему
        if (obj == null)
        {
            return false;
        }

        // Если объект не является комнатой, то он не равен текущему
        if (!(obj is Room))
        {
            return false;
        }

        // Если объект является комнатой, то сравниваем его с текущим по полю RoomID
        var other = (Room)obj;
        return this.RoomId == other.RoomId;
    }

    public override int GetHashCode()
    {
        // Возвращаем хеш-код поля RoomID
        return this.RoomId.GetHashCode();
    }
}
