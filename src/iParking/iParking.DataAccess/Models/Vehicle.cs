using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Vehicle
{
    public int Id { get; set; }

    public string Plate { get; set; } = null!;

    public long Time { get; set; }

    public string Parking { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public byte State { get; set; }

    public int ParkingId { get; set; }

    public virtual Parking ParkingNavigation { get; set; } = null!;
}
