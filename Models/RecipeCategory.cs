using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace navami.Models;

public partial class RecipeCategory
{
    public int RecipeCategoryId { get; set; }

    public string RecipeCategoryName { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<RecipeCategoryMapping> RecipeCategoryMappings { get; set; } = new List<RecipeCategoryMapping>();
}
