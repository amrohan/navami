using navami.Models;

namespace navami.Dto

{
    public class RecipeMasterDto
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


        //etc 
        public string? RecipeCategories { get; set; }
        public List<RawMaterialUsageDto> RawMaterialUsage { get; set; } = new List<RawMaterialUsageDto>();
        public List<RecipeCategoryMappingDto> RecipeCategory { get; set; } = new List<RecipeCategoryMappingDto>();

    }

}