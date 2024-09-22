
namespace navami.Dto
{
    public class RmmasterDto
    {
        public int Rmid { get; set; }

        public string Rmcode { get; set; } = null!;

        public string Rmname { get; set; } = null!;

        public bool IsNewRm { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; } = null!;

        public string? SpecificationNo { get; set; }

        public string? Description { get; set; }

        public bool IsDiscontinued { get; set; }

        public DateTime AddedOn { get; set; }

        public Guid AddedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public string? Party { get; set; }
        public Guid? VendorId { get; set; }

        public decimal Price { get; set; }

        public DateTime? PriceDate { get; set; }

        public bool IsActive { get; set; }
    }

}