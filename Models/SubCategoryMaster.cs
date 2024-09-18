using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class SubCategoryMaster
{
    public int SubCategoryId { get; set; }

    public string SubCategoryName { get; set; } = null!;

    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool IsActive { get; set; }

    public virtual CategoryMaster Category { get; set; } = null!;

    public virtual ICollection<Rmmaster> Rmmasters { get; set; } = new List<Rmmaster>();
}
