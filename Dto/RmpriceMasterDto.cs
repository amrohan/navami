using navami.Components.Pages.RawMaterial;

namespace navami.Dto
{
    public class RmpriceMasterDto
    {
        public int RawMaterialPriceId { get; set; }

        public int RawMaterialId { get; set; }

        public string SupplierName { get; set; } = null!;

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CreatedBy { get; set; }

        public bool IsActive { get; set; }

        public int? UpdatedBy { get; set; }

        public int? VendorId { get; set; }

        public virtual RawMaterial RawMaterial { get; set; } = null!;



    }
}