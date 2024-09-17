using System;
using System.Collections.Generic;

namespace WebAppKafka.Models;

public partial class RiderLocationLog
{
    public Guid Id { get; set; }

    public Guid? RiderId { get; set; }

    public string? GroupId { get; set; }

    public string? Coordinates { get; set; }

    public virtual Rider? Rider { get; set; }
}
