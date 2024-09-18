

using System.ComponentModel.DataAnnotations;

namespace navami.Dto
{

    public class CategoryMasterDto
    {
        public int CategoryId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Category Name is required")]
        public string? CategoryName { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedDate { get; set; }
        public string? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

    }
}