using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class RawMaterialUsage
{
    public int RawMaterialUsageId { get; set; }

    public int RecipeId { get; set; }

    public int RawMaterialId { get; set; }

    public decimal Quantity { get; set; }

    public decimal Cost { get; set; }

    public bool IsActive { get; set; }

    public virtual RawMaterial RawMaterial { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
