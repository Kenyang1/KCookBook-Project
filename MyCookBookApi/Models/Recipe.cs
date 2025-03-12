using System.Text.Json.Serialization;

namespace MyCookBookApi.Models
{
    public class Recipe
    {
        public string RecipeId { get; set; }  // Auto-generated unique ID
        public string Name { get; set; }
        public string TagLine { get; set; }
        public string Summary { get; set; }
        public List<string> Instructions { get; set; } = new List<string>();
        public List<string> Ingredients { get; set; } = new List<string>();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] 
        public List<CategoryType>? Categories { get; set; } = new List<CategoryType>();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]  
        public List<RecipeMedia>? Media { get; set; } = new List<RecipeMedia>();

        public Recipe() {}
    }

}
