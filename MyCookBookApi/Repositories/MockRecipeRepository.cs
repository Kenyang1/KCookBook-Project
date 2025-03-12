using MyCookBookApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyCookBookApi.Repositories
{
    public class MockRecipeRepository : IRecipeRepository
    {
        private readonly List<Recipe> _recipes = new()
        {
            new Recipe
            {
                RecipeId = Guid.NewGuid().ToString(),
                Name = "Pasta",
                TagLine = "Classic Italian Delight",
                Summary = "A simple yet delicious pasta dish with tomato sauce.",
                Ingredients = new List<string>
                {
                    "Pasta", 
                    "Tomato Sauce" 
                },
                Instructions = new List<string>
                {
                    "Boil water and cook pasta until al dente.",
                    "Heat tomato sauce in a pan.",
                    "Mix pasta with the sauce and serve hot."
                },
                Categories = new List<CategoryType> { CategoryType.Dinner, CategoryType.Vegetarian },
                Media = null
            },
            new Recipe
            {
                RecipeId = Guid.NewGuid().ToString(),
                Name = "Salad",
                TagLine = "Fresh and Healthy",
                Summary = "A light and refreshing salad made with fresh vegetables.",
                Ingredients = new List<string>
                {
                    "Lettuce", 
                    "Tomatoes", 
                    "Cucumbers"
                },
                Instructions = new List<string>
                {
                    "Wash and chop all vegetables.",
                    "Mix in a bowl and toss with dressing.",
                    "Serve fresh."
                },
                Categories = new List<CategoryType> { CategoryType.Lunch, CategoryType.Vegan },
                Media = null
            },
            new Recipe
            {
                RecipeId = Guid.NewGuid().ToString(),
                Name = "Omelette",
                TagLine = "Perfect Breakfast Option",
                Summary = "A fluffy and delicious egg omelette.",
                Ingredients = new List<string>
                {
                    "Eggs", 
                    "Milk", 
                    "Cheese"
                },
                Instructions = new List<string>
                {
                    "Beat eggs with milk in a bowl.",
                    "Heat a pan and pour the mixture.",
                    "Add cheese, cook until firm, and fold."
                },
                Categories = new List<CategoryType> { CategoryType.Breakfast, CategoryType.HighProtein },
                Media = null
            }
        };

         public List<Recipe> GetAllRecipes() => _recipes;

        public Recipe GetRecipeById(string id) => _recipes.FirstOrDefault(r => r.RecipeId == id);

        public List<Recipe> SearchRecipes(RecipeSearchRequest searchRequest)
        {
            return _recipes
                .Where(r =>
                    (string.IsNullOrEmpty(searchRequest.Keyword) ||
                    r.Name.Contains(searchRequest.Keyword, StringComparison.OrdinalIgnoreCase) ||
                    r.Summary.Contains(searchRequest.Keyword, StringComparison.OrdinalIgnoreCase)) &&
                    ((searchRequest.Categories == null || searchRequest.Categories.Count == 0) || 
                    r.Categories.Any(c => searchRequest.Categories.Contains(c)))
                )
                .ToList();
        }

        public void AddRecipe(Recipe recipe)
        {
            if (string.IsNullOrWhiteSpace(recipe.Name) || recipe.Ingredients == null || !recipe.Ingredients.Any())
                throw new System.ArgumentException("Invalid recipe data");

            _recipes.Add(recipe);
        }

        public bool UpdateRecipe(string id, Recipe updatedRecipe)
        {
            var index = _recipes.FindIndex(r => r.RecipeId == id);
            if (index == -1) return false;
            _recipes[index] = updatedRecipe;
            return true;
        }

        public bool DeleteRecipe(string id)
        {
            var recipe = GetRecipeById(id);
            if (recipe == null) return false;
            _recipes.Remove(recipe);
            return true;
        }
    }
}
