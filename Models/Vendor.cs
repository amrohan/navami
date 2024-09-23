using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class Vendor
{
    public Guid VendorId { get; set; }

    public string VendorName { get; set; } = null!;

    public bool IsActive { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
