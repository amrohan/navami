
using navami.Models;

namespace navami.Dto
{
    public class RecipeCategoryMappingDto
    {
        public int RecipeCategoryMappingId { get; set; }

        public int RecipeId { get; set; }

        public int RecipeCategoryId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public Recipe Recipe { get; set; } = null!;

        public RecipeCategory RecipeCategory { get; set; } = null!;
    }
    //public int RecipeCategoryMappingId { get; set; }

    //public int RecipeId { get; set; }

    //public int RecipeCategoryId { get; set; }
}
