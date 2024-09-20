using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class RecipeCategoryMapping
{
    public int RecipeCategoryMappingId { get; set; }

    public int RecipeId { get; set; }

    public int RecipeCategoryId { get; set; }

    public virtual RecipeMaster Recipe { get; set; } = null!;

    public virtual RecipeCategory RecipeCategory { get; set; } = null!;
}
