
using navami.Models;

namespace navami.Dto
{
    public class RawMaterialsDto
    {
        public Guid RawMaterialId { get; set; }

        public string RawMaterialCode { get; set; } = null!;

        public string RawMaterialName { get; set; } = null!;

        public bool IsNew { get; set; }

        public Guid CategoryId { get; set; }

        public Guid SubCategoryId { get; set; }

        public string? SpecificationNo { get; set; }

        public string? Description { get; set; }

        public bool IsDiscontinued { get; set; }

        public DateTime AddedOn { get; set; }

        public Guid AddedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public string? Party { get; set; }

        public decimal Price { get; set; }

        public DateTime? PriceDate { get; set; }

        public bool IsActive { get; set; }

        public Guid? VendorId { get; set; }

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<RawMaterialPrice> RawMaterialPrices { get; set; } = new List<RawMaterialPrice>();

        public virtual ICollection<RawMaterialUsage> RawMaterialUsages { get; set; } = new List<RawMaterialUsage>();

        public virtual SubCategory SubCategory { get; set; } = null!;

        public string CategoryName { get; set; } = null!;
        public string SubCategoryName { get; set; } = null!;

        //public int Rmid { get; set; }

        //public string Rmcode { get; set; } = null!;

        //public string Rmname { get; set; } = null!;

        //public bool IsNewRm { get; set; }

        //public int CategoryId { get; set; }
        //public string CategoryName { get; set; } = null!;

        //public int SubCategoryId { get; set; }
        //public string SubCategoryName { get; set; } = null!;

        //public string? SpecificationNo { get; set; }

        //public string? Description { get; set; }

        //public bool IsDiscontinued { get; set; }

        //public DateTime AddedOn { get; set; }

        //public Guid AddedBy { get; set; }

        //public DateTime? LastModifiedOn { get; set; }

        //public Guid? LastModifiedBy { get; set; }

        //public string? Party { get; set; }
        //public Guid? VendorId { get; set; }

        //public decimal Price { get; set; }

        //public DateTime? PriceDate { get; set; }

        //public bool IsActive { get; set; }
    }

}