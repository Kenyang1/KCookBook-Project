using MyCookBookApi.Models;
using MyCookBookApi.Repositories;
using System.Collections.Generic;

namespace MyCookBookApi.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public List<Recipe> GetAllRecipes() => _recipeRepository.GetAllRecipes();

        public Recipe GetRecipeById(string id) => _recipeRepository.GetRecipeById(id);

        public List<Recipe> SearchRecipes(RecipeSearchRequest searchRequest) => _recipeRepository.SearchRecipes(searchRequest);

        public void AddRecipe(Recipe recipe) => _recipeRepository.AddRecipe(recipe);

public bool UpdateRecipe(string id, Recipe recipe) => 
    _recipeRepository.UpdateRecipe(id, recipe);

        public bool DeleteRecipe(string id) => _recipeRepository.DeleteRecipe(id);

    }
}
