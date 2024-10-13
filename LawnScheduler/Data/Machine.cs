using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace LawnScheduler.Data;

public partial class Machine
{
    public int MachineId { get; set; }

    public string MachineName { get; set; } = null!;

    public string Model { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string OperatorId { get; set; } = null!;


    public IdentityUser IdentityUser { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
