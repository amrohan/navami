using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class RecipeCategoryMapping
{
    public int RecipeCategoryMappingId { get; set; }

    public int RecipeId { get; set; }

    public int RecipeCategoryId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual RecipeCategory RecipeCategory { get; set; } = null!;
}
