using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string RoomName { get; set; } = null!;

    public string? Capacity { get; set; }

    public int UserId { get; set; }

    public DateOnly? ReservationDate { get; set; }

    public int? ReservationDuration { get; set; }

    public TimeOnly? StartTime { get; set; }

    public string ReservationStatus { get; set; } = null!;

    public string RoomStatus { get; set; } = null!;

    public bool? IsReserved { get; set; }

    public TimeOnly? EndTime { get; set; }

    public virtual UserTable User { get; set; } = null!;
}
