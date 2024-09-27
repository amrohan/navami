using navami.Models;
using System.ComponentModel.DataAnnotations;

namespace navami.Dto
{
    public class SubCategoryDto
    {
        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; } = null!;

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; }

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<RawMaterial> RawMaterials { get; set; } = new List<RawMaterial>();

    }

}