namespace Pizzeria.Tests.Utils;

public static class TestFileHelper
{
    public static string GetResourcePath(string relativePath)
    {
        var basePath = AppContext.BaseDirectory;
        return Path.Combine(basePath, "Resources", relativePath);
    }
}