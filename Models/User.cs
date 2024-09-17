using System;
using System.Collections.Generic;

namespace navami.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string? Username { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public string? Mobile { get; set; }

    public string? Password { get; set; }

    public bool? IsDeactivated { get; set; }

    public virtual ICollection<RecipeCategory> RecipeCategoryCreatedByNavigations { get; set; } = new List<RecipeCategory>();

    public virtual ICollection<RecipeCategory> RecipeCategoryUpdatedByNavigations { get; set; } = new List<RecipeCategory>();
}
