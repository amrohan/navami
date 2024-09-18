using System.ComponentModel.DataAnnotations;

namespace navami
{
    public class VendorMasterDto
    {
        public Guid VendorId { get; set; }

        [Required]
        public string VendorName { get; set; } = null!;

        public bool IsActive { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}