using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class RmpriceMaster
{
    public int RmpriceId { get; set; }

    public int Rmid { get; set; }

    public string SupplierName { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public bool IsActive { get; set; }

    public Guid? UpdatedBy { get; set; }

    public Guid? VendorId { get; set; }

    public virtual Rmmaster Rm { get; set; } = null!;
}
