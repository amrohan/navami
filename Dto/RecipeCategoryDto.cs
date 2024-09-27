
using navami.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace navami.Dto
{
    public class RecipeCategoryDto
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecipeCategoryId { get; set; }

        public string RecipeCategoryName { get; set; } = null!;

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

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