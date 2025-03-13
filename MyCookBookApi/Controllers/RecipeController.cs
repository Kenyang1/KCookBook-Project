using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MyCookBookApi.Models;
using MyCookBookApi.Services;

namespace MyCookBookApi.Controllers 
{

    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipeController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Recipe>> GetAllRecipes()
        {
            return Ok(_recipeService.GetAllRecipes());
        }

        [HttpGet("{id}")]
        public ActionResult<Recipe> GetRecipeById(string id)
        {
            var recipe = _recipeService.GetRecipeById(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return Ok(recipe);
        }

        [HttpPost("search")]
        public ActionResult<IEnumerable<Recipe>> SearchRecipes([FromBody] RecipeSearchRequest searchRequest)
        {
            if (searchRequest == null || string.IsNullOrWhiteSpace(searchRequest.Keyword))
            {
                return BadRequest("Invalid search request.");
            }

            // Ensure Categories is never null
            searchRequest.Categories ??= new List<CategoryType>();

            var recipes = _recipeService.SearchRecipes(searchRequest);
            return Ok(recipes);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteRecipe(string id) 
        {
            var deleted = _recipeService.DeleteRecipe(id);
            if (!deleted) 
            {
                return NotFound(); // If the recipe does not exist
            }
            return NoContent(); // Successfully deleted
        }


        [HttpPut("{id}")]
        public IActionResult UpdateRecipe(string id, [FromBody] Recipe recipe) 
        {
            if (recipe == null || string.IsNullOrWhiteSpace(recipe.Name)) 
            {
                return BadRequest("Invalid recipe data.");
            }

            var updated = _recipeService.UpdateRecipe(id, recipe);
            if (!updated) 
            {
                return NotFound(); // Recipe doesn't exist
            }
            return NoContent(); // Successfully updated
        }

        [HttpPost]
        public ActionResult<Recipe> CreateRecipe([FromBody] Recipe recipe)
        {
            if (recipe == null || string.IsNullOrWhiteSpace(recipe.Name))
            {
                return BadRequest("Invalid recipe data.");
            }

            // Ensure RecipeId is created in the backend
            recipe.RecipeId = Guid.NewGuid().ToString();

            _recipeService.AddRecipe(recipe);
            return CreatedAtAction(nameof(GetRecipeById), new { id = recipe.RecipeId }, recipe);
        }

    }
}