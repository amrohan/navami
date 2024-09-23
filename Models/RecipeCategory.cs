using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class RecipeCategory
{
    public Guid RecipeCategoryId { get; set; }

    public string RecipeCategoryName { get; set; } = null!;

    public Guid CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<RecipeCategoryMapping> RecipeCategoryMappings { get; set; } = new List<RecipeCategoryMapping>();
}
