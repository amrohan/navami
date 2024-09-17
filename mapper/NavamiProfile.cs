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
        }
    }
}
