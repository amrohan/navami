using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class CategoryMaster
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Rmmaster> Rmmasters { get; set; } = new List<Rmmaster>();

    public virtual ICollection<SubCategoryMaster> SubCategoryMasters { get; set; } = new List<SubCategoryMaster>();
}
