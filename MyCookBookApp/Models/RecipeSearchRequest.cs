namespace MyCookBookApp.Models
{
    public class RecipeSearchRequest
    {
        public string Keyword { get; set; }
        public List<CategoryType> Categories { get; set; } = new List<CategoryType>();
    }
}
