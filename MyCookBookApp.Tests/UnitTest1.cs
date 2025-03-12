namespace MyCookBookApp.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {

    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("P", false)]
    [InlineData("Pasta", true)]
    [InlineData("ThisRecipeNameIsWayTooLongAndInvalid", false)]
    public void ValidateRecipeName_ReturnsExpectedResults(string name, bool expected)
    {
        var result = RecipeValidator.ValidateRecipeName(name);
        Assert.Equal(expected, result);
    }

}
