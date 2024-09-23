using AutoMapper;
using navami.Dto;
using navami.Models;

namespace navami.mapper
{
    public class NavamiProfile : Profile
    {
        public NavamiProfile()
        {
            CreateMap<UserMaster, UserDto>();
            CreateMap<UserDto, UserMaster>();
            CreateMap<RecipeCategory, RecipeCategoryDto>();
            CreateMap<RecipeCategoryDto, RecipeCategory>();
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<SubCategory, SubCategoryDto>();
            CreateMap<SubCategoryDto, SubCategory>();
            CreateMap<Vendor, VendorMasterDto>();
            CreateMap<VendorMasterDto, Vendor>();
            CreateMap<RawMaterial, RawMaterialsDto>();
            CreateMap<RawMaterialsDto, RawMaterial>();
            CreateMap<RawMaterialsDto, RawMaterialPrice>();
            CreateMap<RawMaterialPrice, RawMaterialsDto>();
            CreateMap<Recipe, RecipeMasterDto>();
            CreateMap<RecipeMasterDto, Recipe>();
            CreateMap<RawMaterialUsage, RawMaterialUsageDto>();
            CreateMap<RawMaterialUsageDto, RawMaterialUsage>();
            CreateMap<RecipeCategoryMapping, RecipeCategoryMappingDto>();
            CreateMap<RecipeCategoryMappingDto, RecipeCategoryMapping>();
        }
    }
}
