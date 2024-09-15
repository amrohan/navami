using navami.Models;

namespace navami.Services
{
    public class RecipeService
    {
        private readonly List<Recipe> _recipes = new();

        public List<Recipe> GetAllRecipes() => _recipes;

        public Recipe GetRecipeById(int id) => _recipes.FirstOrDefault(r => r.Id == id);

        public void AddRecipe(Recipe recipe)
        {
            recipe.Id = _recipes.Count + 1;
            _recipes.Add(recipe);
        }

        public void UpdateRecipe(Recipe recipe)
        {
            var existingRecipe = GetRecipeById(recipe.Id);
            if (existingRecipe != null)
            {
                existingRecipe.Name = recipe.Name;
                existingRecipe.Description = recipe.Description;
                existingRecipe.Ingredients = recipe.Ingredients;
            }
        }

        public void DeleteRecipe(int id) => _recipes.RemoveAll(r => r.Id == id);

    }
};
