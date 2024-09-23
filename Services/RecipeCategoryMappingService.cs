using AutoMapper;
using Microsoft.EntityFrameworkCore;
using navami.Dto;
using navami.Models;

namespace navami
{
    public class RecipeCategoryMappingService
    {

        private readonly NavamiDevContext dbContext;
        private readonly IMapper _mapper;
        public RecipeCategoryMappingService(NavamiDevContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }

        public ApiResponse<List<RecipeCategoryMappingDto>> GetRecipeCategoryMapping()
        {
            try
            {
                var recipeCategoryMapping = dbContext.RecipeCategoryMappings.ToList();
                return new ApiResponse<List<RecipeCategoryMappingDto>>(_mapper.Map<List<RecipeCategoryMappingDto>>(recipeCategoryMapping));
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<RecipeCategoryMappingDto>>(ex.Message);
            }
        }

        //GetRecipeCategoryMappingById
        public ApiResponse<RecipeCategoryMappingDto> GetRecipeCategoryMappingById(Guid id)
        {
            try
            {
                var recipeCategoryMapping = dbContext.RecipeCategoryMappings
                    .Where(u => u.RecipeCategoryMappingId == id)
                    .Include(u => u.Recipe)
                    .Include(u => u.RecipeCategory)
                    .FirstOrDefault();
                if (recipeCategoryMapping == null)
                {
                    return new ApiResponse<RecipeCategoryMappingDto>("RecipeCategoryMapping not found");
                }
                var recipeCategoryMappingDto = _mapper.Map<RecipeCategoryMappingDto>(recipeCategoryMapping);
                return new ApiResponse<RecipeCategoryMappingDto>(recipeCategoryMappingDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<RecipeCategoryMappingDto>(ex.Message);
            }
        }

        // getRecipeCategoryMappingByRecipeId
        public ApiResponse<List<RecipeCategoryMappingDto>> GetRecipeCategoryMappingByRecipeId(Guid recipeId)
        {
            try
            {
                var recipeCategoryMapping = dbContext.RecipeCategoryMappings
                    .Where(u => u.RecipeId == recipeId)
                    .Include(u => u.Recipe)
                    .Include(u => u.RecipeCategory)
                    .ToList();
                if (recipeCategoryMapping == null)
                {
                    return new ApiResponse<List<RecipeCategoryMappingDto>>("RecipeCategoryMapping not found");
                }
                var recipeCategoryMappingDto = _mapper.Map<List<RecipeCategoryMappingDto>>(recipeCategoryMapping);
                return new ApiResponse<List<RecipeCategoryMappingDto>>(recipeCategoryMappingDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<RecipeCategoryMappingDto>>(ex.Message);
            }
        }

        //deleteRecipeCategoryMapping
        public ApiResponse<RecipeCategoryMappingDto> DeleteRecipeCategoryMapping(Guid id)
        {
            try
            {
                var recipeCategoryMapping = dbContext.RecipeCategoryMappings.FirstOrDefault(u => u.RecipeCategoryMappingId == id);
                if (recipeCategoryMapping == null)
                {
                    return new ApiResponse<RecipeCategoryMappingDto>("RecipeCategoryMapping not found");
                }
                // recipeCategoryMapping.IsActive = true;
                dbContext.RecipeCategoryMappings.Remove(recipeCategoryMapping);

                dbContext.SaveChanges();
                return new ApiResponse<RecipeCategoryMappingDto>(_mapper.Map<RecipeCategoryMappingDto>(recipeCategoryMapping));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RecipeCategoryMappingDto>(ex.Message);
            }
        }

    }
}