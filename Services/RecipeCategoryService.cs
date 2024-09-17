
using AutoMapper;
using navami.Dto;
using navami.Models;

namespace navami
{

    public class RecipeCategoryService
    {
        private readonly NavamiContext dbContext;
        private readonly IMapper _mapper;

        public RecipeCategoryService(NavamiContext context, IMapper mapper)
        {
            dbContext = context;
            _mapper = mapper;
        }

        // get all
        public ApiResponse<List<RecipeCategoryDto>> GetRecipeCategories()
        {
            try
            {
                var recipeCategories = dbContext.RecipeCategories.Where(u => u.IsActive != true).ToList();
                // return the list of RecipeCategories as RecipeCategoryDto objects use mapper
                return new ApiResponse<List<RecipeCategoryDto>>(_mapper.Map<List<RecipeCategoryDto>>(recipeCategories));
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<RecipeCategoryDto>>(ex.Message);
            }
        }


        // get by id
        public ApiResponse<RecipeCategoryDto> GetRecipeCategoryById(int id)
        {
            try
            {
                var recipeCategory = dbContext.RecipeCategories.FirstOrDefault(u => u.Id == id);
                if (recipeCategory == null)
                {
                    return new ApiResponse<RecipeCategoryDto>("Recipe Category does not exists.");
                }

                return new ApiResponse<RecipeCategoryDto>(_mapper.Map<RecipeCategoryDto>(recipeCategory));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RecipeCategoryDto>(ex.Message);
            }
        }
        // add
        public ApiResponse<RecipeCategoryDto> AddRecipeCategory(RecipeCategoryDto model)
        {
            try
            {
                // Check if the RecipeCategoryName already exists
                var existingRecipeCategory = dbContext.RecipeCategories.FirstOrDefault(u => u.RecipeCategoryName == model.RecipeCategoryName);
                if (existingRecipeCategory != null)
                {
                    return new ApiResponse<RecipeCategoryDto>("Recipe Category already exists.");
                }

                // Add the RecipeCategory to the database
                var recipeCategory = _mapper.Map<RecipeCategory>(model);
                dbContext.RecipeCategories.Add(recipeCategory);
                dbContext.SaveChanges();

                return new ApiResponse<RecipeCategoryDto>(_mapper.Map<RecipeCategoryDto>(recipeCategory)); // Return the registered RecipeCategory
            }
            catch (Exception ex)
            {
                return new ApiResponse<RecipeCategoryDto>(ex.Message); // Return any exception message
            }
        }

        // update 
        // public ApiResponse<RecipeCategoryDto> UpdateRecipeCategory(RecipeCategoryDto model)
        // {
        //     try
        //     {
        //         // Check if the RecipeCategoryName already exists
        //         var existingRecipeCategory = dbContext.RecipeCategories.FirstOrDefault(u => u.RecipeCategoryName == model.RecipeCategoryName);
        //         if (existingRecipeCategory != null)
        //         {
        //             return new ApiResponse<RecipeCategoryDto>("Recipe Category already exists.");
        //         }

        //         // Add the RecipeCategory to the database
        //         var recipeCategory = _mapper.Map<RecipeCategory>(model);
        //         dbContext.SaveChanges();

        //         return new ApiResponse<RecipeCategoryDto>(_mapper.Map<RecipeCategoryDto>(recipeCategory));
        //     }
        //     catch (Exception ex)
        //     {
        //         return new ApiResponse<RecipeCategoryDto>(ex.Message); // Return any exception message
        //     }
        // }
        public ApiResponse<RecipeCategoryDto> UpdateRecipeCategory(RecipeCategoryDto model)
        {
            try
            {
                // Fetch the existing entity from the database
                var existingRecipeCategory = dbContext.RecipeCategories
                    .FirstOrDefault(u => u.Id == model.Id);

                if (existingRecipeCategory == null)
                {
                    return new ApiResponse<RecipeCategoryDto>("Recipe Category not found.");
                }

                // Check if the RecipeCategoryName already exists (excluding the current entity)
                var nameExists = dbContext.RecipeCategories
                    .Any(u => u.RecipeCategoryName == model.RecipeCategoryName && u.Id != model.Id);

                if (nameExists)
                {
                    return new ApiResponse<RecipeCategoryDto>("Recipe Category already exists.");
                }

                // Map the updated properties
                _mapper.Map(model, existingRecipeCategory);

                // Save changes
                dbContext.SaveChanges();

                return new ApiResponse<RecipeCategoryDto>(_mapper.Map<RecipeCategoryDto>(existingRecipeCategory));
            }
            catch (Exception ex)
            {
                return new ApiResponse<RecipeCategoryDto>(ex.Message); // Return any exception message
            }
        }

        // delete
        public ApiResponse<RecipeCategory> DeleteRecipeCategory(int id)
        {
            try
            {
                // Check if the RecipeCategoryName already exists
                var existingRecipeCategory = dbContext.RecipeCategories.FirstOrDefault(u => u.Id == id);
                if (existingRecipeCategory == null)
                {
                    return new ApiResponse<RecipeCategory>("Recipe Category does not exists.");
                }

                // Add the RecipeCategory to the database
                // dbContext.RecipeCategories.Remove(existingRecipeCategory);
                existingRecipeCategory.IsActive = true;

                dbContext.SaveChanges();

                return new ApiResponse<RecipeCategory>(existingRecipeCategory); // Return the registered RecipeCategory
            }
            catch (Exception ex)
            {
                return new ApiResponse<RecipeCategory>(ex.Message); // Return any exception message
            }
        }

        public ApiResponse<RecipeCategory> GetRecipeCategoryByName(string name)
        {
            try
            {
                var recipeCategory = dbContext.RecipeCategories.FirstOrDefault(u => u.RecipeCategoryName == name);
                if (recipeCategory == null)
                {
                    return new ApiResponse<RecipeCategory>("Recipe Category does not exists.");
                }

                return new ApiResponse<RecipeCategory>(recipeCategory);
            }
            catch (Exception ex)
            {
                return new ApiResponse<RecipeCategory>(ex.Message);
            }
        }

    }
}