
using navami.Models;
using System.ComponentModel.DataAnnotations;

namespace navami.Dto
{
    public class RecipeCategoryDto
    {
        public Guid RecipeCategoryId { get; set; }

        public string RecipeCategoryName { get; set; } = null!;

        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public List<RecipeCategoryMapping> RecipeCategoryMappings { get; set; } = new List<RecipeCategoryMapping>();

        //public int RecipeCategoryId { get; set; }
        //[Required(ErrorMessage = "Recipe Category Name is required")]
        //public string RecipeCategoryName { get; set; } = null!;
        //public string CreatedAt { get; set; } = null!;
        //public string UpdatedAt { get; set; } = null!;
        //public string CreatedBy { get; set; } = null!;
        //public string? UpdatedBy { get; set; }
        //public bool IsActive { get; set; }


    }
}