
using System.ComponentModel.DataAnnotations;

namespace navami
{
    public class RecipeCategoryDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Recipe Category Name is required")]
        public string RecipeCategoryName { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;
        public string UpdatedAt { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; }


    }
}