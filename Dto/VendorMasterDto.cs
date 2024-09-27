using System.ComponentModel.DataAnnotations;

namespace navami
{
    public class VendorMasterDto
    {
        public int VendorId { get; set; }

        [Required]
        public string VendorName { get; set; } = null!;

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}