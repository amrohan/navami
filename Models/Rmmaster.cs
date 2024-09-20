using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class Rmmaster
{
    public int Rmid { get; set; }

    public string Rmcode { get; set; } = null!;

    public string Rmname { get; set; } = null!;

    public bool IsNewRm { get; set; }

    public int CategoryId { get; set; }

    public int SubCategoryId { get; set; }

    public string? SpecificationNo { get; set; }

    public string? Description { get; set; }

    public bool IsDiscontinued { get; set; }

    public DateTime AddedOn { get; set; }

    public Guid AddedBy { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public Guid? LastModifiedBy { get; set; }

    public string? Party { get; set; }

    public decimal? Price { get; set; }

    public DateTime? PriceDate { get; set; }

    public bool IsActive { get; set; }

    public Guid? VendorId { get; set; }

    public virtual CategoryMaster Category { get; set; } = null!;

    public virtual ICollection<RawMaterialUsage> RawMaterialUsages { get; set; } = new List<RawMaterialUsage>();

    public virtual ICollection<RmpriceMaster> RmpriceMasters { get; set; } = new List<RmpriceMaster>();

    public virtual SubCategoryMaster SubCategory { get; set; } = null!;
}
