using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using navami.Dto;
using navami.Models;

namespace navami
{
    public class RecipeMasterService
    {

        private readonly NavamiDevContext dbContext;
        private readonly IMapper _mapper;
        public RecipeMasterService(NavamiDevContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }


        //public async Task<ApiResponse<List<RecipeMasterDto>>> GetAllRecipeMasters()
        //{
        //    // Fetch all recipe masters along with their raw materials and category mappings
        //    var recipeMasters = await dbContext.Recipes
        //        .Where(r => r.IsActive != true)
        //        .Include(rm => rm.RawMaterialUsages)
        //        .Include(rm => rm.RecipeCategoryMappings)
        //            .ThenInclude(rcm => rcm.RecipeCategory) // Assuming you have a navigation property for RecipeCategory
        //        .ToListAsync();

        //    var recipeMasterDtos = _mapper.Map<List<RecipeMasterDto>>(recipeMasters);

        //    foreach (var recipeMasterDto in recipeMasterDtos)
        //    {
        //        // Map raw materials
        //        recipeMasterDto.RawMaterialUsages = _mapper.Map<List<RawMaterialUsageDto>>(recipeMasters(rm => rm.RecepId == recipeMasterDto.RecipeId).RawMaterialUsages);

        //        // Get category names from the mapped RecipeCategoryMappings
        //        var categoryNames = recipeMasters.Find(rm => rm.RecipeId == recipeMasterDto.RecipeId)
        //            .RecipeCategoryMappings.Select(rcm =>
        //                dbContext.RecipeCategories.FirstOrDefault(rc => rc.RecipeCategoryId == rcm.RecipeCategoryId)?.RecipeCategoryName)
        //            .Where(name => name != null); // Filter out any null names

        //        // Concatenate the category names and assign to RecipeCategories
        //        recipeMasterDto.RecipeCategories = string.Join(", ", categoryNames);
        //    }

        //    return new ApiResponse<List<RecipeMasterDto>>(recipeMasterDtos);
        //}

        public async Task<ApiResponse<List<RecipeMasterDto>>> GetAllRecipeMasters()
        {
            // Fetch all recipe masters along with their raw materials and category mappings
            var recipeMasters = await dbContext.Recipes
                .Where(r => r.IsActive != true)
                .Include(rm => rm.RawMaterialUsages)
                .Include(rm => rm.RecipeCategoryMappings)
                    .ThenInclude(rcm => rcm.RecipeCategory) // Assuming you have a navigation property for RecipeCategory
                .ToListAsync();

            var recipeMasterDtos = _mapper.Map<List<RecipeMasterDto>>(recipeMasters);

            foreach (var recipeMasterDto in recipeMasterDtos)
            {
                // Map raw materials
                recipeMasterDto.RawMaterialUsage = _mapper.Map<List<RawMaterialUsageDto>>(
                    recipeMasters.FirstOrDefault(rm => rm.RecipeId == recipeMasterDto.RecipeId)?.RawMaterialUsages);

                // Get category names from the mapped RecipeCategoryMappings
                var categoryNames = recipeMasters.FirstOrDefault(rm => rm.RecipeId == recipeMasterDto.RecipeId)
                    ?.RecipeCategoryMappings.Select(rcm =>
                        dbContext.RecipeCategories.FirstOrDefault(rc => rc.RecipeCategoryId == rcm.RecipeCategoryId)?.RecipeCategoryName)
                    .Where(name => name != null);

                // Concatenate the category names and assign to RecipeCategories
                recipeMasterDto.RecipeCategories = string.Join(", ", categoryNames);
            }

            return new ApiResponse<List<RecipeMasterDto>>(recipeMasterDtos);
        }

        // GetRecipeMasterById
        public async Task<ApiResponse<RecipeMasterDto>> GetRecipeMasterById(Guid recipeId)
        {
            // Fetch the recipe master along with related data using LINQ with Include
            var recipeMaster = await dbContext.Recipes
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
            foreach (var item in recipeMasterDto.RawMaterialUsage)
            {
                // Fetch category and subcategory details based on Rmid
                var rawMaterial = await dbContext.RawMaterials.FindAsync(item.RawMaterialId);

                if (rawMaterial != null)
                {
                    item.RawMaterialName = rawMaterial.RawMaterialName;
                    var category = await dbContext.Categories.FindAsync(rawMaterial.CategoryId);
                    item.CategoryName = category?.CategoryName;

                    var subCategory = await dbContext.SubCategories.FindAsync(rawMaterial.SubCategoryId);
                    item.SubCategoryName = subCategory?.SubCategoryName;
                }
            }
            
            recipeMasterDto.RecipeCategory = _mapper.Map<List<RecipeCategoryMappingDto>>(recipeMaster.Categories);

            return new ApiResponse<RecipeMasterDto>(recipeMasterDto);
        }



        // AddRecipe
        public async Task<ApiResponse<RecipeMasterDto>> AddRecipeMaster(RecipeMasterDto recipeMasterDto)
        {
            var recipeMaster = _mapper.Map<Recipe>(recipeMasterDto);
            dbContext.Recipes.Add(recipeMaster);

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

        public async Task UpdateRecipeCostAsync(Guid recipeId)
        {
            await dbContext.Database.ExecuteSqlRawAsync("EXEC UpdateRecipeCost @RecipeId", new SqlParameter("@RecipeId", recipeId));
        }

        public async Task<ApiResponse<RecipeMasterDto>> UpdateRecipeMaster(RecipeMasterDto recipeMasterDto)
        {
            try
            {
                var recipeMaster = await dbContext.Recipes
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
                        .FirstOrDefault(u => u.RawMaterialUsageId == rawMaterial.RawMaterialUsageId);
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
        public async Task<ApiResponse<RecipeMasterDto>> DeleteRecipeCategoryMapping(Guid recipeId, Guid recipeCategoryId)
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
        public async Task<ApiResponse<RecipeMasterDto>> DeleteRecipeMaster(Guid recipeId)
        {
            try
            {
                var recipeMaster = await dbContext.Recipes.FirstOrDefaultAsync(u => u.RecipeId == recipeId);
                if (recipeMaster == null)
                {
                    return new ApiResponse<RecipeMasterDto>("Recipe not found");
                }

                // dbContext.RecipeMasters.Remove(recipeMaster);
                recipeMaster.IsActive = true;
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