

using navami.Components.Pages.RawMaterial;
using navami.Components.Pages.SubCategory;
using System.ComponentModel.DataAnnotations;

namespace navami.Dto
{

    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public virtual List<RawMaterial> RawMaterials { get; set; } = new List<RawMaterial>();

        public virtual List<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

    }
}