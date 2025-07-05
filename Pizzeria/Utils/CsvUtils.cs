namespace Pizzeria.Utils;

/// <summary>
/// Utility methods for CSV parsing.
/// </summary>
public static class CsvUtils
{
    /// <summary>
    /// Detects the most likely delimiter in a CSV header line.
    /// Defaults to comma (',') unless only semicolons (';') are present.
    /// </summary>
    /// <param name="headerLine">The first line of the CSV file, typically the header.</param>
    /// <returns>The detected delimiter character, usually ',' or ';'.</returns>
    public static char DetectDelimiter(string headerLine)
    {
        if (headerLine.Contains(';') && !headerLine.Contains(','))
            return ';';

        return ',';
    }
}