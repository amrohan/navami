using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class RawMaterial
{
    public Guid RawMaterialId { get; set; }

    public string RawMaterialCode { get; set; } = null!;

    public string RawMaterialName { get; set; } = null!;

    public bool IsNew { get; set; }

    public Guid CategoryId { get; set; }

    public Guid SubCategoryId { get; set; }

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

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<RawMaterialPrice> RawMaterialPrices { get; set; } = new List<RawMaterialPrice>();

    public virtual ICollection<RawMaterialUsage> RawMaterialUsages { get; set; } = new List<RawMaterialUsage>();

    public virtual SubCategory SubCategory { get; set; } = null!;
}
