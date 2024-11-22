using System;
using System.Collections.Generic;

namespace LawnScheduler.Data;

public partial class Booking
{
    public int BookingId { get; set; }

    public int MachineId { get; set; }

    public string CustomerId { get; set; } = null!;

    public DateTime BookingDate { get; set; }

    public DateTime ScheduledDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsConfirmed { get; set; }

    public bool IsConflict { get; set; }

    public DateTime? CompletionDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Machine Machine { get; set; } = null!;
}
