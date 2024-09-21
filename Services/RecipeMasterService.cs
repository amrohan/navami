using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using navami.Dto;
using navami.Models;

namespace navami
{
    public class RecipeMasterService
    {

        private readonly NavamiContext dbContext;
        private readonly IMapper _mapper;
        public RecipeMasterService(NavamiContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }


        public async Task<ApiResponse<List<RecipeMasterDto>>> GetAllRecipeMasters()
        {
            // Fetch all recipe masters along with their raw materials and category mappings
            var recipeMasters = await dbContext.RecipeMasters
                .Include(rm => rm.RawMaterialUsages)
                .Include(rm => rm.RecipeCategoryMappings)
                    .ThenInclude(rcm => rcm.RecipeCategory) // Assuming you have a navigation property for RecipeCategory
                .ToListAsync();

            var recipeMasterDtos = _mapper.Map<List<RecipeMasterDto>>(recipeMasters);

            foreach (var recipeMasterDto in recipeMasterDtos)
            {
                // Map raw materials
                recipeMasterDto.RawMaterialUsage = _mapper.Map<List<RawMaterialUsageDto>>(recipeMasters.Find(rm => rm.RecipeId == recipeMasterDto.RecipeId).RawMaterialUsages);

                // Get category names from the mapped RecipeCategoryMappings
                var categoryNames = recipeMasters.Find(rm => rm.RecipeId == recipeMasterDto.RecipeId)
                    .RecipeCategoryMappings.Select(rcm =>
                        dbContext.RecipeCategories.FirstOrDefault(rc => rc.RecipeCategoryId == rcm.RecipeCategoryId)?.RecipeCategoryName)
                    .Where(name => name != null); // Filter out any null names

                // Concatenate the category names and assign to RecipeCategories
                recipeMasterDto.RecipeCategories = string.Join(", ", categoryNames);
            }

            return new ApiResponse<List<RecipeMasterDto>>(recipeMasterDtos);
        }


        // GetRecipeMasterById
        public async Task<ApiResponse<RecipeMasterDto>> GetRecipeMasterById(int recipeId)
        {
            // Fetch the recipe master along with related data using LINQ with Include
            var recipeMaster = await dbContext.RecipeMasters
                .Where(r => r.RecipeId == recipeId)
                .Select(r => new
                {
                    Recipe = r,
                    RawMaterials = r.RawMaterialUsages,
                    Categories = r.RecipeCategoryMappings
                })
                .FirstOrDefaultAsync();

            if (recipeMaster == null)
            {
                return new ApiResponse<RecipeMasterDto>("Recipe not found.");
            }

            // Map the recipe master entity to DTO
            var recipeMasterDto = _mapper.Map<RecipeMasterDto>(recipeMaster.Recipe);
            recipeMasterDto.RawMaterialUsage = _mapper.Map<List<RawMaterialUsageDto>>(recipeMaster.RawMaterials);
            recipeMasterDto.RecipeCategory = _mapper.Map<List<RecipeCategoryMappingDto>>(recipeMaster.Categories);

            return new ApiResponse<RecipeMasterDto>(recipeMasterDto);
        }



        // AddRecipe
        public async Task<ApiResponse<RecipeMasterDto>> AddRecipeMaster(RecipeMasterDto recipeMasterDto)
        {
            var recipeMaster = _mapper.Map<RecipeMaster>(recipeMasterDto);
            dbContext.RecipeMasters.Add(recipeMaster);

            // Save related raw materials
            foreach (var rawMaterial in recipeMasterDto.RawMaterialUsage)
            {
                var rawMaterialUsage = _mapper.Map<RawMaterialUsage>(rawMaterial);
                rawMaterialUsage.RecipeId = recipeMaster.RecipeId;
                recipeMaster.RawMaterialUsages.Add(rawMaterialUsage);
            }

            // Save related recipe categories
            foreach (var recipeCategory in recipeMasterDto.RecipeCategory)
            {
                var recipeCategoryMapping = _mapper.Map<RecipeCategoryMapping>(recipeCategory);
                recipeCategoryMapping.RecipeId = recipeMaster.RecipeId;
                recipeMaster.RecipeCategoryMappings.Add(recipeCategoryMapping);
            }

            await dbContext.SaveChangesAsync();

            // Call to update the recipe cost after saving the recipe
            await UpdateRecipeCostAsync(recipeMaster.RecipeId);

            return new ApiResponse<RecipeMasterDto>(_mapper.Map<RecipeMasterDto>(recipeMaster));
        }

        public async Task UpdateRecipeCostAsync(int recipeId)
        {
            await dbContext.Database.ExecuteSqlRawAsync("EXEC UpdateRecipeCost @RecipeID", new SqlParameter("@RecipeID", recipeId));
        }

        public async Task<ApiResponse<RecipeMasterDto>> UpdateRecipeMaster(RecipeMasterDto recipeMasterDto)
        {
            try
            {
                var recipeMaster = await dbContext.RecipeMasters
                    .Include(r => r.RawMaterialUsages)
                    .Include(r => r.RecipeCategoryMappings)
                    .FirstOrDefaultAsync(u => u.RecipeId == recipeMasterDto.RecipeId);

                if (recipeMaster == null)
                {
                    return new ApiResponse<RecipeMasterDto>("Recipe not found");
                }

                // Map general properties
                _mapper.Map(recipeMasterDto, recipeMaster);

                // Update related raw materials
                foreach (var rawMaterial in recipeMasterDto.RawMaterialUsage)
                {
                    var rawMaterialUsage = recipeMaster.RawMaterialUsages
                        .FirstOrDefault(u => u.RmusageId == rawMaterial.RmusageId);
                    if (rawMaterialUsage == null)
                    {
                        // Add new raw material
                        rawMaterialUsage = _mapper.Map<RawMaterialUsage>(rawMaterial);
                        rawMaterialUsage.RecipeId = recipeMaster.RecipeId;
                        recipeMaster.RawMaterialUsages.Add(rawMaterialUsage);
                    }
                    else
                    {
                        rawMaterialUsage.Quantity = rawMaterial.Quantity;
                        rawMaterialUsage.Cost = rawMaterial.Cost;
                    }
                }

                // Update related recipe categories
                foreach (var recipeCategory in recipeMasterDto.RecipeCategory)
                {
                    var recipeCategoryMapping = recipeMaster.RecipeCategoryMappings
                        .FirstOrDefault(u => u.RecipeCategoryId == recipeCategory.RecipeCategoryId);
                    if (recipeCategoryMapping == null)
                    {
                        // Add new recipe category
                        recipeCategoryMapping = _mapper.Map<RecipeCategoryMapping>(recipeCategory);
                        recipeCategoryMapping.RecipeId = recipeMaster.RecipeId;
                        recipeMaster.RecipeCategoryMappings.Add(recipeCategoryMapping);
                    }
                    else
                    {
                        // Update existing recipe category (excluding the key)
                        // recipeCategoryMapping. = recipeCategory.CreatedAt; 
                        // recipeCategoryMapping.IsActive = recipeCategory.IsActive;

                    }
                }

                await dbContext.SaveChangesAsync();

                // Call to update the recipe cost after saving the recipe
                await UpdateRecipeCostAsync(recipeMaster.RecipeId);

                return new ApiResponse<RecipeMasterDto>(_mapper.Map<RecipeMasterDto>(recipeMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RecipeMasterDto>(ex.Message);
            }
        }

        // DeleteRecipeCategoryMapping(recipeMasterDto.RecipeId, removedCategory.RecipeCategoryId)
        public async Task<ApiResponse<RecipeMasterDto>> DeleteRecipeCategoryMapping(int recipeId, int recipeCategoryId)
        {
            try
            {
                var recipeCategoryMapping = dbContext.RecipeCategoryMappings
                    .FirstOrDefault(u => u.RecipeId == recipeId && u.RecipeCategoryId == recipeCategoryId);
                if (recipeCategoryMapping == null)
                {
                    return new ApiResponse<RecipeMasterDto>("RecipeCategoryMapping not found");
                }

                dbContext.RecipeCategoryMappings.Remove(recipeCategoryMapping);
                await dbContext.SaveChangesAsync();

                return new ApiResponse<RecipeMasterDto>(_mapper.Map<RecipeMasterDto>(recipeCategoryMapping));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RecipeMasterDto>(ex.Message);
            }
        }

        // deletereipe
        public async Task<ApiResponse<RecipeMasterDto>> DeleteRecipeMaster(int recipeId)
        {
            try
            {
                // var recipeMaster = dbContext.RecipeMasters
                //     .Include(r => r.RawMaterialUsages)
                //     .Include(r => r.RecipeCategoryMappings)
                //     .FirstOrDefault(u => u.RecipeId == recipeId);
                var recipeMaster = await dbContext.RecipeMasters.FirstOrDefaultAsync(u => u.RecipeId == recipeId);
                if (recipeMaster == null)
                {
                    return new ApiResponse<RecipeMasterDto>("Recipe not found");
                }

                // dbContext.RecipeMasters.Remove(recipeMaster);
                recipeMaster.IsActive = false;
                await dbContext.SaveChangesAsync();

                return new ApiResponse<RecipeMasterDto>(_mapper.Map<RecipeMasterDto>(recipeMaster));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RecipeMasterDto>(ex.Message);
            }
        }

    }
}