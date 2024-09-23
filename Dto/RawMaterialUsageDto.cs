using navami.Models;

namespace navami.Dto
{
    public partial class RawMaterialUsageDto
    {
        public Guid RawMaterialUsageId { get; set; }

        public Guid RecipeId { get; set; }

        public Guid RawMaterialId { get; set; }

        public decimal Quantity { get; set; }

        public decimal Cost { get; set; }

        public virtual RawMaterial RawMaterial { get; set; } = null!;

        public virtual Recipe Recipe { get; set; } = null!;

        // etc 

        public string CategoryName { get; set; }
        public string RawMaterialName { get; set; } = null!;
        public string SubCategoryName { get; set; }

        //public int RmusageId { get; set; }

        //public int RecipeId { get; set; }

        //public string Rmname { get; set; } = null!;

        //public string CategoryName { get; set; }

        //public string SubCategoryName { get; set; }

        //public int Rmid { get; set; }

        //public decimal Quantity { get; set; }

        //public decimal Cost { get; set; }
        //public virtual RecipeMaster Recipe { get; set; } = null!;

        //public virtual Rmmaster Rm { get; set; } = null!;
    }

}