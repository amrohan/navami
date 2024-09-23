
using navami.Models;

namespace navami.Dto
{
    public class RecipeCategoryMappingDto
    {
        public Guid RecipeCategoryMappingId { get; set; }

        public Guid RecipeId { get; set; }

        public Guid RecipeCategoryId { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public virtual Recipe Recipe { get; set; } = null!;

        public virtual RecipeCategory RecipeCategory { get; set; } = null!;
    }
    //public int RecipeCategoryMappingId { get; set; }

    //public int RecipeId { get; set; }

    //public int RecipeCategoryId { get; set; }
}
