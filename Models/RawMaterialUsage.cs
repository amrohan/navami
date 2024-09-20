using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class RawMaterialUsage
{
    public int RmusageId { get; set; }

    public int RecipeId { get; set; }

    public int Rmid { get; set; }

    public decimal Quantity { get; set; }

    public decimal Cost { get; set; }

    public virtual RecipeMaster Recipe { get; set; } = null!;

    public virtual Rmmaster Rm { get; set; } = null!;
}
