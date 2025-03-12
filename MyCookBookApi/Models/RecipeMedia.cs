namespace MyCookBookApi.Models
{
    public class RecipeMedia
    {
        public string Url { get; set; }  // Firebase Storage URL
        public string Type { get; set; }  // "image" or "video"
        public int Order { get; set; }  // Display order
    }
}
