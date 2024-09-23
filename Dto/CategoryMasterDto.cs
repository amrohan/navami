

using navami.Components.Pages.RawMaterial;
using navami.Components.Pages.SubCategory;
using System.ComponentModel.DataAnnotations;

namespace navami.Dto
{

    public class CategoryDto
    {
        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public Guid CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<RawMaterial> RawMaterials { get; set; } = new List<RawMaterial>();

        public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();

    }
}