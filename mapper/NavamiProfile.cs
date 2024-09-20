using AutoMapper;
using navami.Dto;
using navami.Models;

namespace navami.mapper
{
    public class NavamiProfile : Profile
    {
        public NavamiProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<RecipeCategory, RecipeCategoryDto>();
            CreateMap<RecipeCategoryDto, RecipeCategory>();
            CreateMap<CategoryMaster, CategoryMasterDto>();
            CreateMap<CategoryMasterDto, CategoryMaster>();
            CreateMap<SubCategoryMaster, SubCategoryMasterDto>();
            CreateMap<SubCategoryMasterDto, SubCategoryMaster>();
            CreateMap<VendorMaster, VendorMasterDto>();
            CreateMap<VendorMasterDto, VendorMaster>();
            CreateMap<Rmmaster, RmmasterDto>();
            CreateMap<RmmasterDto, Rmmaster>();
            CreateMap<RmpriceMasterDto, RmpriceMaster>();
            CreateMap<RmpriceMaster, RmpriceMasterDto>();
            CreateMap<RecipeMaster, RecipeMasterDto>();
            CreateMap<RecipeMasterDto, RecipeMaster>();
            CreateMap<RawMaterialUsage, RawMaterialUsageDto>();
            CreateMap<RawMaterialUsageDto, RawMaterialUsage>();
            CreateMap<RecipeCategoryMapping, RecipeCategoryMappingDto>();
            CreateMap<RecipeCategoryMappingDto, RecipeCategoryMapping>();
        }
    }
}
