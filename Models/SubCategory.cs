using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class SubCategory
{
    public Guid SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<RawMaterial> RawMaterials { get; set; } = new List<RawMaterial>();
}
