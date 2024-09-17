using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class RecipeCategory
{
    public int Id { get; set; }

    public string RecipeCategoryName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
