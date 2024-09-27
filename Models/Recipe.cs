using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public string RecipeName { get; set; } = null!;

    public string? Profile { get; set; }

    public string Username { get; set; } = null!;

    public decimal AdjustedCost { get; set; }

    public decimal TotalCost { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<RawMaterialUsage> RawMaterialUsages { get; set; } = new List<RawMaterialUsage>();

    public virtual ICollection<RecipeCategoryMapping> RecipeCategoryMappings { get; set; } = new List<RecipeCategoryMapping>();
}
