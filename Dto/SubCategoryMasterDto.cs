using System.ComponentModel.DataAnnotations;

namespace navami.Dto
{
    public class SubCategoryMasterDto
    {
        public int SubCategoryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Sub Category Name is required")]
        public string? SubCategoryName { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedDate { get; set; }
        public string? UpdatedDate { get; set; }
        public bool IsActive { get; set; }

    }

}