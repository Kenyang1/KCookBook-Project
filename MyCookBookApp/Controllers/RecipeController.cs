using Microsoft.AspNetCore.Mvc;
using MyCookBookApp.Services;
using System.Threading.Tasks;
using MyCookBookApp.Models;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyCookBookApp.Controllers
{
    [ApiController]
    [Route("Recipe")]
    public class RecipeController : Controller
    {
        private readonly RecipeService _recipeService;

        public RecipeController(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        // Show the Recipe Index Page
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        // Fetch All Recipes (GET /Recipe/GetAll)
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllRecipes()
        {
            var recipes = await _recipeService.GetRecipesAsync();
            return Json(recipes);
        }

        // Fetch Recipe by ID (GET /Recipe/{id})
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(string id)
        {
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                return NotFound(new { success = false, message = "Recipe not found" });
            }
            return Json(recipe);
        }

        // Search for Recipes (POST /Recipe/Search)
        [HttpPost("Search")]
        public async Task<IActionResult> SearchRecipes([FromBody] RecipeSearchRequest searchRequest)
        {
            // searchRequest.Categories = new List<CategoryType>();
            var recipes = await _recipeService.SearchRecipesAsync(searchRequest);
            return Json(recipes);
        }

        // Add a Recipe (POST /Recipe/Add)
        [HttpPost("Add")]
        public async Task<IActionResult> AddRecipe([FromBody] Recipe recipe)
        {
            Console.WriteLine("Received Recipe: " + JsonConvert.SerializeObject(recipe));

            // TODO: Add Validation 
            // if (recipe == null || string.IsNullOrWhiteSpace(recipe.Name) || 
            //     recipe.Ingredients == null || recipe.Ingredients.Count == 0 ||
            //     recipe.Instructions == null || recipe.Instructions.Count == 0 ||
            //     string.IsNullOrWhiteSpace(recipe.Summary) || recipe.Categories == null)
            // {
            //     return BadRequest(new { success = false, message = "Invalid recipe data" });
            // }

            bool added = await _recipeService.AddRecipeAsync(recipe);
            return Json(new { success = added, message = added ? "Recipe added successfully" : "Failed to add recipe" });
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> EditRecipe(string id, [FromBody] Recipe recipe) 
        {
            if (recipe == null || 
                string.IsNullOrWhiteSpace(recipe.Name) || 
                recipe.Ingredients == null || recipe.Ingredients.Count == 0 || 
                recipe.Instructions == null || recipe.Instructions.Count == 0 || 
                string.IsNullOrWhiteSpace(recipe.Summary) || 
                recipe.Categories == null) 
            {
                return BadRequest(new { success = false, message = "Invalid recipe data" });
            }

            bool updated = await _recipeService.UpdateRecipeAsync(recipe);
            
            return Json(new { success = updated, message = updated ? "Recipe updated successfully" : "Failed to update recipe" });
        }


   }
}
