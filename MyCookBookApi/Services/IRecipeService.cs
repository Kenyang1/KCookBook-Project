using MyCookBookApi.Models;
using System.Collections.Generic;

namespace MyCookBookApi.Services
{
    public interface IRecipeService
    {
        List<Recipe> GetAllRecipes();
        Recipe GetRecipeById(string id);
        List<Recipe> SearchRecipes(RecipeSearchRequest searchRequest);
        void AddRecipe(Recipe recipe);
        bool UpdateRecipe(string id, Recipe recipe);
        bool DeleteRecipe(string id);
    }
}