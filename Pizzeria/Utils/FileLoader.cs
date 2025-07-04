
namespace Pizzeria.Utils;

public static class FileLoader
{
    public static async Task<string> ReadFileAsync(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        return await File.ReadAllTextAsync(path);
    }
}